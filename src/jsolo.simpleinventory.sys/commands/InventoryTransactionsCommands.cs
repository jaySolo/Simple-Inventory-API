using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

using jsolo.simpleinventory.core.entities;
using jsolo.simpleinventory.core.enums;
using jsolo.simpleinventory.core.objects;
using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.handlers;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.extensions;
using jsolo.simpleinventory.sys.models;

using MediatR;


namespace jsolo.simpleinventory.sys.commands.InventoryTransactions;



public class CreateInventoryTransactionCommand : IRequest<DataOperationResult<InventoryTransactionViewModel>>
{
    public required InventoryTransactionViewModel NewInventoryTransaction { get; set; }
}



public class CreateInventoryTransactionCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<CreateInventoryTransactionCommand, DataOperationResult<InventoryTransactionViewModel>>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult<InventoryTransactionViewModel>> Handle(CreateInventoryTransactionCommand request, CancellationToken token)
    {
        Inventory? inventory = Context.Inventories.FirstOrDefault(inventory => inventory.Id.Equals(request.NewInventoryTransaction.Inventory.Id));
        InventoryTransactionType type = (InventoryTransactionType)request.NewInventoryTransaction.Type.Value;

        if (inventory is null)
        {
            return Task.FromResult(DataOperationResult<InventoryTransactionViewModel>.InvalidData);
        }
        if (Context.InventoryTransactions.Any(inventoryTransaction =>
            inventoryTransaction.Inventory.Id.Equals(request.NewInventoryTransaction.Inventory.Id) &&
            inventoryTransaction.Type.Equals(type) &&
            inventoryTransaction.Date.Equals(request.NewInventoryTransaction.TimeStamp) &&
            inventoryTransaction.Amount.Equals(request.NewInventoryTransaction.Amount)
        ))
        {
            return Task.FromResult(DataOperationResult<InventoryTransactionViewModel>.Exists);
        }

        try
        {
            var inventoryTransaction = new InventoryTransaction(
                Guid.NewGuid(),
                request.NewInventoryTransaction.TimeStamp,
                type,
                inventory,
                request.NewInventoryTransaction.Amount,
                createdOn: DateTime.Now,
                creatorId: CurrentUser.UserId.ToString()
            );

            Context.BeginTransaction()
                .Add(inventoryTransaction)
                .SaveChanges()
                .CloseTransaction();

            return Task.FromResult(DataOperationResult<InventoryTransactionViewModel>.Success(inventoryTransaction.ToViewModel()));
        }
        catch (Exception ex)
        {
            Context.RollbackChanges().CloseTransaction();

            return Task.FromResult(DataOperationResult<InventoryTransactionViewModel>.Failure(
                ex.ToString()
            ));
        }
    }
}


public class UpdateInventoryTransactionCommand : IRequest<DataOperationResult<InventoryTransactionViewModel>>
{
    public required Guid InventoryTransactionId { get; set; }

    public required InventoryTransactionViewModel UpdatedInventoryTransaction { get; set; }
}


public class UpdateInventoryTransactionCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<CreateInventoryTransactionCommand, DataOperationResult<InventoryTransactionViewModel>>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult<InventoryTransactionViewModel>> Handle(CreateInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        return base.Handle(request, cancellationToken);
    }
}



public class DeleteInventoryTransactionCommand : IRequest<DataOperationResult>
{
    public required Guid InventoryTransactionId { get; set; }
}



public class DeleteInventoryTransactionCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<DeleteInventoryTransactionCommand, DataOperationResult>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult> Handle(DeleteInventoryTransactionCommand request, CancellationToken token)
    {
        var inventoryTransaction = Context.InventoryTransactions.FirstOrDefault(inventoryTransaction => inventoryTransaction.Id.Equals(request.InventoryTransactionId)) ?? null;

        if (inventoryTransaction is null)
        {
            return Task.FromResult(DataOperationResult.NotFound);
        }

        try
        {
            Context.BeginTransaction()
                .Delete(inventoryTransaction)
                .SaveChanges()
                .CloseTransaction();

            return Task.FromResult(DataOperationResult.Success);
        }
        catch (Exception ex)
        {
            Context.RollbackChanges().CloseTransaction();

            return Task.FromResult(DataOperationResult.Failure(
                ex.ToString()
            ));
        }
    }
}

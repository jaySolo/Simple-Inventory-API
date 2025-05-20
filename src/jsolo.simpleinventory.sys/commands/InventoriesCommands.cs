using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

using jsolo.simpleinventory.core.entities;
using jsolo.simpleinventory.core.objects;
using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.handlers;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.extensions;
using jsolo.simpleinventory.sys.models;

using MediatR;


namespace jsolo.simpleinventory.sys.commands.Inventories;



public class CreateInventoryCommand : IRequest<DataOperationResult<InventoryViewModel>>
{
    public required InventoryViewModel NewInventory { get; set; }
}



public class CreateInventoryCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<CreateInventoryCommand, DataOperationResult<InventoryViewModel>>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult<InventoryViewModel>> Handle(CreateInventoryCommand request, CancellationToken token)
    {
        Product product = Context.Products.FirstOrDefault(product => product.Id.Equals(request.NewInventory.Product.Id));

        if (product is null)
        {
            return Task.FromResult(DataOperationResult<InventoryViewModel>.InvalidData);
        }
        if (Context.Inventories.Any(inventory => inventory.Item.Id.Equals(request.NewInventory.Product.Id)))
        {
            return Task.FromResult(DataOperationResult<InventoryViewModel>.Exists);
        }

        try
        {
            var inventory = new Inventory(
                Guid.NewGuid(),
                product,
                stockCount: request.NewInventory.StockCount,
                minimumQty: request.NewInventory.MinimumStockCount,
                reorderQty: request.NewInventory.MinimumReorderQuantity,
                createdOn: DateTime.Now,
                creatorId: CurrentUser.UserId.ToString()
            );

            Context.BeginTransaction()
                .Add(inventory)
                .SaveChanges()
                .CloseTransaction();

            return Task.FromResult(DataOperationResult<InventoryViewModel>.Success(inventory.ToViewModel()));
        }
        catch (Exception ex)
        {
            Context.RollbackChanges().CloseTransaction();

            return Task.FromResult(DataOperationResult<InventoryViewModel>.Failure(
                ex.ToString()
            ));
        }
    }
}


public class UpdateInventoryCommand : IRequest<DataOperationResult<InventoryViewModel>>
{
    public required Guid InventoryId { get; set; }

    public required InventoryViewModel UpdatedInventory { get; set; }
}


public class UpdateInventoryCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<CreateInventoryCommand, DataOperationResult<InventoryViewModel>>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult<InventoryViewModel>> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
    {
        return base.Handle(request, cancellationToken);
    }
}



public class DeleteInventoryCommand : IRequest<DataOperationResult>
{
    public required Guid InventoryId { get; set; }
}



public class DeleteInventoryCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<DeleteInventoryCommand, DataOperationResult>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult> Handle(DeleteInventoryCommand request, CancellationToken token)
    {
        var inventory = Context.Inventories.FirstOrDefault(inventory => inventory.Id.Equals(request.InventoryId)) ?? null;

        if (inventory is null)
        {
            return Task.FromResult(DataOperationResult.NotFound);
        }

        try
        {
            Context.BeginTransaction()
                .Delete(inventory)
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

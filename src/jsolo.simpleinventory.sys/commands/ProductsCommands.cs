using jsolo.simpleinventory.core.entities;
using jsolo.simpleinventory.core.objects;
using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.handlers;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.extensions;
using jsolo.simpleinventory.sys.models;

using MediatR;
using System.Linq;

namespace jsolo.simpleinventory.sys.commands.Products;



public class CreateProductCommand : IRequest<DataOperationResult<ProductViewModel>>
{
    public required ProductViewModel NewProduct { get; set; }
}



public class CreateProductCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<CreateProductCommand, DataOperationResult<ProductViewModel>>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult<ProductViewModel>> Handle(CreateProductCommand request, CancellationToken token)
    {
        if (Context.Products.Any(product =>
            product.ProductName.Equals(request.NewProduct.Name, StringComparison.InvariantCultureIgnoreCase)))
        {
            return Task.FromResult(DataOperationResult<ProductViewModel>.Exists);
        }

        try
        {
            var newId = (Context.Products.OrderByDescending(product => product.Id).FirstOrDefault()?.Id ?? 0) + 1;
            var type = Context.ProductTypes.SingleOrDefault(t => t.Name.Equals(request.NewProduct.Type, StringComparison.InvariantCultureIgnoreCase));
            var currency = Context.Currencies.FirstOrDefault(c => c.Code.Equals(request.NewProduct.MarketValue.Currency.Code ?? ""));
            List<Vendor> suppliers = [];
            suppliers.AddRange(from supplier in request.NewProduct.Suppliers
                               let vendor = Context.Vendors.FirstOrDefault(v => v.Id == supplier.Id)
                               where vendor is not null
                               select vendor);
            
            var product = new Product(
                newId,
                request.NewProduct.InternalProductNumber,
                request.NewProduct.ExternalProductNumber,
                request.NewProduct.Name,
                type,
                request.NewProduct.Make,
                request.NewProduct.Description,
                new Money(request.NewProduct.MarketValue.Amount, currency),
                DateTime.Now,
                CurrentUser.UserId.ToString(),
                suppliers: suppliers
            );

            Context.BeginTransaction()
                .Add(product)
                .SaveChanges()
                .CloseTransaction();

            return Task.FromResult(DataOperationResult<ProductViewModel>.Success(
                product.ToViewModel()
            ));

        }
        catch (Exception ex)
        {
            Context.RollbackChanges().CloseTransaction();

            return Task.FromResult(DataOperationResult<ProductViewModel>.Failure(
                ex.ToString()
            ));
        }
    }
}



public class UpdateProductCommand : IRequest<DataOperationResult<ProductViewModel>>
{
    public required int ProductId { get; set; }

    public required ProductViewModel UpdatedProduct { get; set; }
}



public class UpdateProductCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<UpdateProductCommand, DataOperationResult<ProductViewModel>>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult<ProductViewModel>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        return base.Handle(request, cancellationToken);
    }
}



public class DeleteProductCommand : IRequest<DataOperationResult>
    {
        public required int ProductId { get; set; }
    }



public class DeleteProductCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<DeleteProductCommand, DataOperationResult>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult> Handle(DeleteProductCommand request, CancellationToken token)
    {
        var product = Context.Products.FirstOrDefault(product => product.Id == request.ProductId);

        if (product is null)
        {
            return Task.FromResult(DataOperationResult.NotFound);
        }

        try
        {
            Context.BeginTransaction()
                .Delete(product)
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

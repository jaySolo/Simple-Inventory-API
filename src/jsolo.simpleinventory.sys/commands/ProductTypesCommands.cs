using jsolo.simpleinventory.core.objects;
using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.handlers;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.extensions;
using jsolo.simpleinventory.sys.models;

using MediatR;


namespace jsolo.simpleinventory.sys.commands.ProductTypes;



public class CreateProductTypeCommand : IRequest<DataOperationResult<ProductTypeViewModel>>
{
    public required ProductTypeViewModel NewProductType { get; set; }
}



public class CreateProductTypeCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<CreateProductTypeCommand, DataOperationResult<ProductTypeViewModel>>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult<ProductTypeViewModel>> Handle(CreateProductTypeCommand request, CancellationToken token)
    {
        if (Context.ProductTypes.Any(productType => productType.Name.Contains(request.NewProductType.Name, StringComparison.InvariantCultureIgnoreCase)))
        {
            return Task.FromResult(DataOperationResult<ProductTypeViewModel>.Exists);
        }

        try
        {
            var productType = new ProductType(
                name: request.NewProductType.Name,
                description: request.NewProductType.Description ?? ""
            );

            Context.BeginTransaction()
                .Add(productType)
                .SaveChanges()
                .CloseTransaction();

            return Task.FromResult(DataOperationResult<ProductTypeViewModel>.Success(
                productType.ToViewModel()
            ));

        }
        catch (Exception ex)
        {
            Context.RollbackChanges().CloseTransaction();

            return Task.FromResult(DataOperationResult<ProductTypeViewModel>.Failure(
                ex.ToString()
            ));
        }
    }
}

public class UpdateProductTypeCommand : IRequest<DataOperationResult<ProductTypeViewModel>>
{
    public required string ProductTypeName { get; set; }

    public required ProductTypeViewModel UpdatedProductType { get; set; }
}



public class UpdatedProductTypeCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<CreateProductTypeCommand, DataOperationResult<ProductTypeViewModel>>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult<ProductTypeViewModel>> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
    {
        return base.Handle(request, cancellationToken);
    }
}



public class DeleteProductTypeCommand : IRequest<DataOperationResult>
{
    public required string ProductTypeName { get; set; }
}



public class DeleteProductTypeCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<DeleteProductTypeCommand, DataOperationResult>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult> Handle(DeleteProductTypeCommand request, CancellationToken token)
    {
        var productType = Context.ProductTypes.FirstOrDefault(productType => productType.Name.Contains(request.ProductTypeName, StringComparison.InvariantCultureIgnoreCase)) ?? null;

        if (productType is null)
        {
            return Task.FromResult(DataOperationResult.NotFound);
        }

        try
        {
            Context.BeginTransaction()
                .Delete(productType)
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

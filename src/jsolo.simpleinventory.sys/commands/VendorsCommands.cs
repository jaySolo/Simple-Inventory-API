using jsolo.simpleinventory.core.entities;
using jsolo.simpleinventory.core.objects;
using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.handlers;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.extensions;
using jsolo.simpleinventory.sys.models;

using MediatR;


namespace jsolo.simpleinventory.sys.commands.Vendors;



public class CreateVendorCommand : IRequest<DataOperationResult<VendorViewModel>>
{
    public required VendorViewModel NewVendor { get; set; }
}



public class CreateVendorCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<CreateVendorCommand, DataOperationResult<VendorViewModel>>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult<VendorViewModel>> Handle(CreateVendorCommand request, CancellationToken token)
    {
        if (Context.Vendors.Any(vendor =>
            vendor.CompanyName.Equals(request.NewVendor.BusinessName, StringComparison.InvariantCultureIgnoreCase) &&
            vendor.ContactPersonName.FullName.Equals(
                $"{request.NewVendor.ContactTitle} {request.NewVendor.ContactFirstName} {request.NewVendor.ContactLastName}",
                StringComparison.InvariantCultureIgnoreCase
            )
        ))
        {
            return Task.FromResult(DataOperationResult<VendorViewModel>.Exists);
        }

        try
        {
            var newId = (Context.Vendors.OrderByDescending(vendor => vendor.Id).FirstOrDefault()?.Id ?? 0) + 1;

            var vendor = new Vendor(
                newId,
                request.NewVendor.BusinessName,
                new Name(request.NewVendor.ContactTitle ?? "", request.NewVendor.ContactLastName ?? "", request.NewVendor.ContactFirstName ?? ""),
                request.NewVendor.ContactTelephone,
                request.NewVendor.ContactMobile,
                request.NewVendor.ContactFax,
                request.NewVendor.ContactEmail,
                request.NewVendor.Address,
                DateTime.Now,
                CurrentUser.UserId.ToString()
            );

            Context.BeginTransaction()
                .Add(vendor)
                .SaveChanges()
                .CloseTransaction();

            return Task.FromResult(DataOperationResult<VendorViewModel>.Success(
                vendor.ToViewModel()
            ));

        }
        catch (Exception ex)
        {
            Context.RollbackChanges().CloseTransaction();

            return Task.FromResult(DataOperationResult<VendorViewModel>.Failure(
                ex.ToString()
            ));
        }
    }
}



public class UpdateVendorCommand : IRequest<DataOperationResult<VendorViewModel>>
{
    public required int VendorId { get; set; }

    public required VendorViewModel UpdatedVendor { get; set; }
}



public class UpdateVendorCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<UpdateVendorCommand, DataOperationResult<VendorViewModel>>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult<VendorViewModel>> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        return base.Handle(request, cancellationToken);
    }
}



public class DeleteVendorCommand : IRequest<DataOperationResult>
    {
        public required int VendorId { get; set; }
    }



public class DeleteVendorCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<DeleteVendorCommand, DataOperationResult>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult> Handle(DeleteVendorCommand request, CancellationToken token)
    {
        var vendor = Context.Vendors.FirstOrDefault(vendor => vendor.Id == request.VendorId);

        if (vendor is null)
        {
            return Task.FromResult(DataOperationResult.NotFound);
        }

        try
        {
            Context.BeginTransaction()
                .Delete(vendor)
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

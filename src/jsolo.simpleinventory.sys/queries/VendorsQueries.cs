using MediatR;

using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.extensions;
using jsolo.simpleinventory.sys.models;


namespace jsolo.simpleinventory.sys.queries.Vendors;


public class GetVendorsQuery : IRequest<QueryFilterResultsViewModel<VendorViewModel>>
{
    public VendorsFilterViewModel? Parameters { get; set; }
}



public class GetVendorsQueryHandler(IDatabaseContext context) : IRequestHandler<GetVendorsQuery, QueryFilterResultsViewModel<VendorViewModel>>
{
    private readonly IDatabaseContext _context = context;

    public async Task<QueryFilterResultsViewModel<VendorViewModel>> Handle(GetVendorsQuery req, CancellationToken token = default)
    {
        QueryFilterResultsViewModel<VendorViewModel> getVendors()
        {

            var vendors = _context.Vendors.ToArray();

            List<VendorViewModel> results = [];
            int resultsCount = 0;


            if (req.Parameters is { })
            {

                // filter by supplied parameters
                if (req.Parameters.BusinessName?.Length > 0)
                {
                    vendors = [.. vendors.Where(vendor => vendor.CompanyName.Contains(req.Parameters.BusinessName, StringComparison.InvariantCultureIgnoreCase))];
                }

                if (req.Parameters.ContactName?.Length > 0)
                {
                    vendors = [.. vendors.Where(vendor => vendor.ContactPersonName.FullName.Contains(req.Parameters.ContactName, StringComparison.InvariantCultureIgnoreCase))];
                }

                if (req.Parameters.ContactMobile?.Length > 0)
                {
                    vendors = [.. vendors.Where(vendor => vendor.MobilePhoneNumber.Contains(req.Parameters.ContactMobile, StringComparison.InvariantCultureIgnoreCase))];
                }

                if (req.Parameters.ContactTelephone?.Length > 0)
                {
                    vendors = [.. vendors.Where(vendor => vendor.TelephoneNumber.Contains(req.Parameters.ContactTelephone, StringComparison.InvariantCultureIgnoreCase))];
                }

                if (req.Parameters.ContactFax?.Length > 0)
                {
                    vendors = [.. vendors.Where(vendor => vendor.FascimileNumber.Contains(req.Parameters.ContactFax, StringComparison.InvariantCultureIgnoreCase))];
                }

                if (req.Parameters.ContactEmail?.Length > 0)
                {
                    vendors = [.. vendors.Where(vendor => vendor.EmailAddress.Contains(req.Parameters.ContactEmail, StringComparison.InvariantCultureIgnoreCase))];
                }

                if (req.Parameters.Address?.Length > 0)
                {
                    vendors = [.. vendors.Where(vendor => vendor.PhysicalAddress.Contains(req.Parameters.Address, StringComparison.InvariantCultureIgnoreCase))];
                }


                // apply sort parameters
                var sortDesc = req.Parameters.OrderBy == "DESC";
                vendors = (req.Parameters.SortBy?.ToLower() ?? "") switch
                {
                    "businessName" => sortDesc ? [.. vendors.OrderByDescending(vendor => vendor.CompanyName)] : [.. vendors.OrderBy(vendor => vendor.CompanyName)],

                    "contactName" => sortDesc ? [.. vendors.OrderByDescending(vendor => vendor.ContactPersonName.FullName)] : [.. vendors.OrderBy(vendor => vendor.ContactPersonName.FullName)],

                    "contactMobile" => sortDesc ? [.. vendors.OrderByDescending(vendor => vendor.MobilePhoneNumber)] : [.. vendors.OrderBy(vendor => vendor.MobilePhoneNumber)],

                    "contactTelephone" => sortDesc ? [.. vendors.OrderByDescending(vendor => vendor.TelephoneNumber)] : [.. vendors.OrderBy(vendor => vendor.TelephoneNumber)],

                    "contactFax" => sortDesc ? [.. vendors.OrderByDescending(vendor => vendor.FascimileNumber)] : [.. vendors.OrderBy(vendor => vendor.FascimileNumber)],

                    "contactEmail" => sortDesc ? [.. vendors.OrderByDescending(vendor => vendor.EmailAddress)] : [.. vendors.OrderBy(vendor => vendor.EmailAddress)],

                    "address" => sortDesc ? [.. vendors.OrderByDescending(vendor => vendor.PhysicalAddress)] : [.. vendors.OrderBy(vendor => vendor.PhysicalAddress)],

                    _ => sortDesc ? [.. vendors.OrderByDescending(vendor => vendor.Id)] : [.. vendors.OrderBy(vendor => vendor.Id)],
                };

                resultsCount = results.Count;

                // then filter according to page size
                if (req.Parameters?.PageSize > 0 && req.Parameters?.PageIndex > 0)
                {
                    results.AddRange(vendors.Skip(
                        req.Parameters.PageSize * (req.Parameters.PageIndex - 1)
                    ).Take(req.Parameters.PageSize).Select(vendor => vendor.ToViewModel()));
                }
                else
                {
                    results.AddRange(vendors.Select(vendor => vendor.ToViewModel()));
                }
            }
            else
            {
                resultsCount = vendors.Length;

                results.AddRange(vendors.Select(vendor => vendor.ToViewModel()));
            }

            return new QueryFilterResultsViewModel<VendorViewModel>
            {
                Items = results,
                Total = resultsCount
            };
        }


        return await Task.FromResult(getVendors());
    }
}



public class GetVendorDetailsQuery : IRequest<DataOperationResult<VendorViewModel>>
{
    public required int VendorId { get; set; }
}



public class GetVendorDetailsQueryHandler(IDatabaseContext context) : IRequestHandler<GetVendorDetailsQuery, DataOperationResult<VendorViewModel>>
{
    private readonly IDatabaseContext _context = context;

    public async Task<DataOperationResult<VendorViewModel>> Handle(GetVendorDetailsQuery request, CancellationToken cancellationToken = default)
    {
        DataOperationResult<VendorViewModel> getVendor()
        {
            var vendor = _context.Vendors.FirstOrDefault(vendor => vendor.Id == request.VendorId);

            if (vendor is not null)
            {
                Task.FromResult(DataOperationResult<VendorViewModel>.Success(vendor.ToViewModel()));
            }

            return DataOperationResult<VendorViewModel>.NotFound;
        }

        return await Task.FromResult(getVendor());
    }
}

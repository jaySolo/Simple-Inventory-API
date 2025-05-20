using MediatR;

using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.extensions;
using jsolo.simpleinventory.sys.models;


namespace jsolo.simpleinventory.sys.queries.Inventories;


public class GetInventoriesQuery : IRequest<QueryFilterResultsViewModel<InventoryViewModel>>
{
    public InventoriesFilterViewModel? Parameters { get; set; }
}



public class GetInventoriesQueryHandler(IDatabaseContext context) : IRequestHandler<GetInventoriesQuery, QueryFilterResultsViewModel<InventoryViewModel>>
{
    private readonly IDatabaseContext _context = context;

    public async Task<QueryFilterResultsViewModel<InventoryViewModel>> Handle(GetInventoriesQuery req, CancellationToken token = default)
    {
        QueryFilterResultsViewModel<InventoryViewModel> getInventories()
        {

            var Inventories = _context.Inventories.ToArray();

            List<InventoryViewModel> results = [];
            int resultsCount = 0;


            if (req.Parameters is { })
            {
                // filter by supplied parameters
                if (req.Parameters.InternalProductNumber?.Length > 0)
                {
                    Inventories = [.. Inventories.Where(inventory => inventory.Item.InternalProductNumber.Contains(req.Parameters.InternalProductNumber, StringComparison.InvariantCultureIgnoreCase))];
                }

                if (req.Parameters.ExternalProductNumber?.Length > 0)
                {
                    Inventories = [.. Inventories.Where(inventory => inventory.Item.ExternalProductNumber.Contains(req.Parameters.ExternalProductNumber, StringComparison.InvariantCultureIgnoreCase))];
                }

                if (req.Parameters.ProductName?.Length > 0)
                {
                    Inventories = [.. Inventories.Where(inventory => inventory.Item.ProductName.Contains(req.Parameters.ProductName, StringComparison.InvariantCultureIgnoreCase))];
                }

                // if (req.Parameters.ProductType?.Length > 0)
                // {
                //     Inventories = [.. Inventories.Where(inventory => inventory.Type.ToString().Contains(req.Parameters.Make, StringComparison.InvariantCultureIgnoreCase))];
                // }

                if (req.Parameters.ProductMake?.Length > 0)
                {
                    Inventories = [.. Inventories.Where(inventory => inventory.Item.Make.Contains(req.Parameters.ProductMake, StringComparison.InvariantCultureIgnoreCase))];
                }

                if (req.Parameters.ProductBarcode?.Length > 0)
                {
                    Inventories = [.. Inventories.Where(inventory => inventory.Item.BarCode?.Contains(req.Parameters.ProductBarcode, StringComparison.InvariantCultureIgnoreCase) ?? false)];
                }


                // apply sort parameters
                var sortDesc = req.Parameters.OrderBy == "DESC";
                Inventories = (req.Parameters.SortBy?.ToLower() ?? "") switch
                {
                    "externalinventorynumber" => sortDesc ? [.. Inventories.OrderByDescending(inventory => inventory.Item.ExternalProductNumber)] : [.. Inventories.OrderBy(inventory => inventory.Item.ExternalProductNumber)],

                    "internalinventorynumber" => sortDesc ? [.. Inventories.OrderByDescending(inventory => inventory.Item.InternalProductNumber)] : [.. Inventories.OrderBy(inventory => inventory.Item.InternalProductNumber)],

                    "name" => sortDesc ? [.. Inventories.OrderByDescending(inventory => inventory.Item.ProductName)] : [.. Inventories.OrderBy(inventory => inventory.Item.ProductName)],

                    "type" => sortDesc ? [.. Inventories.OrderByDescending(inventory => inventory.Item.Type)] : [.. Inventories.OrderBy(inventory => inventory.Item.Type.Name)],

                    "make" => sortDesc ? [.. Inventories.OrderByDescending(inventory => inventory.Item.Make)] : [.. Inventories.OrderBy(inventory => inventory.Item.Make)],

                    _ => sortDesc ? [.. Inventories.OrderByDescending(inventory => inventory.Id)] : [.. Inventories.OrderBy(inventory => inventory.Id)],
                };

                resultsCount = results.Count;

                // then filter according to page size
                if (req.Parameters?.PageSize > 0 && req.Parameters?.PageIndex > 0)
                {
                    results.AddRange(Inventories.Skip(
                        req.Parameters.PageSize * (req.Parameters.PageIndex - 1)
                    ).Take(req.Parameters.PageSize).Select(inventory => inventory.ToViewModel()));
                }
                else
                {
                    results.AddRange(Inventories.Select(inventory => inventory.ToViewModel()));
                }
            }
            else
            {
                resultsCount = Inventories.Length;

                results.AddRange(Inventories.Select(inventory => inventory.ToViewModel()));
            }

            return new QueryFilterResultsViewModel<InventoryViewModel>
            {
                Items = results,
                Total = resultsCount
            };
        }


        return await Task.FromResult(getInventories());
    }
}



public class GetInventoryDetailsQuery : IRequest<DataOperationResult<InventoryViewModel>>
{
    public required Guid InventoryId { get; set; }
}



public class GetInventoryDetailsQueryHandler(IDatabaseContext context) : IRequestHandler<GetInventoryDetailsQuery, DataOperationResult<InventoryViewModel>>
{
    private readonly IDatabaseContext _context = context;

    public async Task<DataOperationResult<InventoryViewModel>> Handle(GetInventoryDetailsQuery request, CancellationToken cancellationToken = default)
    {
        DataOperationResult<InventoryViewModel> getInventory()
        {
            var inventory = _context.Inventories.FirstOrDefault(inventory => inventory.Id.Equals(request.InventoryId));

            if (inventory is not null)
            {
                Task.FromResult(DataOperationResult<InventoryViewModel>.Success(inventory.ToViewModel()));
            }

            return DataOperationResult<InventoryViewModel>.NotFound;
        }

        return await Task.FromResult(getInventory());
    }
}

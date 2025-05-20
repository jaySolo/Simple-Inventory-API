using MediatR;

using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.extensions;
using jsolo.simpleinventory.sys.models;


namespace jsolo.simpleinventory.sys.queries.Inventories;


public class GetInventoryTransactionsQuery : IRequest<QueryFilterResultsViewModel<InventoryTransactionViewModel>>
{
    public InventoryTransactionsFilterViewModel? Parameters { get; set; }
}



public class GetInventoryTransactionsQueryHandler(IDatabaseContext context) : IRequestHandler<GetInventoryTransactionsQuery, QueryFilterResultsViewModel<InventoryTransactionViewModel>>
{
    private readonly IDatabaseContext _context = context;

    public async Task<QueryFilterResultsViewModel<InventoryTransactionViewModel>> Handle(GetInventoryTransactionsQuery req, CancellationToken token = default)
    {
        QueryFilterResultsViewModel<InventoryTransactionViewModel> getInventories()
        {

            var Inventories = _context.InventoryTransactions.ToArray();

            List<InventoryTransactionViewModel> results = [];
            int resultsCount = 0;


            if (req.Parameters is { })
            {
                // filter by supplied parameters
                if (!req.Parameters.InventoryId.Equals(Guid.Empty))
                {
                    Inventories = [.. Inventories.Where(inventoryTransaction => inventoryTransaction.Inventory.Id.Equals(req.Parameters.InventoryId))];
                }

                switch (req.Parameters.Start)
                {
                    case not null when req.Parameters.End is not null:
                        {
                            Inventories = [.. Inventories.Where(inventoryTransaction => req.Parameters.Start <= inventoryTransaction.Date && inventoryTransaction.Date <= req.Parameters.End)];
                            break;
                        }

                    case not null:
                        {
                            Inventories = [.. Inventories.Where(inventoryTransaction => req.Parameters.Start <= inventoryTransaction.Date)];
                            break;
                        }

                    default:
                        if (req.Parameters.End is not null)
                        {
                            Inventories = [.. Inventories.Where(inventoryTransaction => inventoryTransaction.Date <= req.Parameters.End)];
                        }

                        break;
                }

                switch (req.Parameters.Minimum)
                {
                    case not null when req.Parameters.Maximum is not null:
                        {
                            Inventories = [.. Inventories.Where(inventoryTransaction => req.Parameters.Minimum <= inventoryTransaction.Amount && inventoryTransaction.Amount <= req.Parameters.Maximum)];
                            break;
                        }

                    case not null:
                        {
                            Inventories = [.. Inventories.Where(inventoryTransaction => req.Parameters.Minimum <= inventoryTransaction.Amount)];
                            break;
                        }

                    default:
                        if (req.Parameters.End is not null)
                        {
                            Inventories = [.. Inventories.Where(inventoryTransaction => inventoryTransaction.Amount <= req.Parameters.Maximum)];
                        }

                        break;
                }


                // apply sort parameters
                var sortDesc = req.Parameters.OrderBy == "DESC";
                Inventories = (req.Parameters.SortBy?.ToLower() ?? "") switch
                {
                    "inventory" => sortDesc ? [.. Inventories.OrderByDescending(inventoryTransaction => inventoryTransaction.Inventory.Id)] : [.. Inventories.OrderBy(inventoryTransaction => inventoryTransaction.Inventory.Id)],

                    "date" => sortDesc ? [.. Inventories.OrderByDescending(inventoryTransaction => inventoryTransaction.Date)] : [.. Inventories.OrderBy(inventoryTransaction => inventoryTransaction.Date)],

                    "amount" => sortDesc ? [.. Inventories.OrderByDescending(inventoryTransaction => inventoryTransaction.Amount)] : [.. Inventories.OrderBy(inventoryTransaction => inventoryTransaction.Amount)],

                    _ => sortDesc ? [.. Inventories.OrderByDescending(inventoryTransaction => inventoryTransaction.Id)] : [.. Inventories.OrderBy(inventoryTransaction => inventoryTransaction.Id)],
                };

                resultsCount = results.Count;

                // then filter according to page size
                if (req.Parameters?.PageSize > 0 && req.Parameters?.PageIndex > 0)
                {
                    results.AddRange(Inventories.Skip(
                        req.Parameters.PageSize * (req.Parameters.PageIndex - 1)
                    ).Take(req.Parameters.PageSize).Select(inventoryTransaction => inventoryTransaction.ToViewModel()));
                }
                else
                {
                    results.AddRange(Inventories.Select(inventoryTransaction => inventoryTransaction.ToViewModel()));
                }
            }
            else
            {
                resultsCount = Inventories.Length;

                results.AddRange(Inventories.Select(inventoryTransaction => inventoryTransaction.ToViewModel()));
            }

            return new QueryFilterResultsViewModel<InventoryTransactionViewModel>
            {
                Items = results,
                Total = resultsCount
            };
        }


        return await Task.FromResult(getInventories());
    }
}



public class GetInventoryTransactionDetailsQuery : IRequest<DataOperationResult<InventoryTransactionViewModel>>
{
    public required Guid InventoryTransactionId { get; set; }
}



public class GetInventoryTransactionDetailsQueryHandler(IDatabaseContext context) : IRequestHandler<GetInventoryTransactionDetailsQuery, DataOperationResult<InventoryTransactionViewModel>>
{
    private readonly IDatabaseContext _context = context;

    public async Task<DataOperationResult<InventoryTransactionViewModel>> Handle(GetInventoryTransactionDetailsQuery request, CancellationToken cancellationToken = default)
    {
        DataOperationResult<InventoryTransactionViewModel> getInventory()
        {
            var inventoryTransaction = _context.InventoryTransactions.FirstOrDefault(inventoryTransaction => inventoryTransaction.Id.Equals(request.InventoryTransactionId));

            if (inventoryTransaction is not null)
            {
                Task.FromResult(DataOperationResult<InventoryTransactionViewModel>.Success(inventoryTransaction.ToViewModel()));
            }

            return DataOperationResult<InventoryTransactionViewModel>.NotFound;
        }

        return await Task.FromResult(getInventory());
    }
}

using MediatR;

using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.extensions;
using jsolo.simpleinventory.sys.models;


namespace jsolo.simpleinventory.sys.queries.ProductTypes;


public class GetProductTypesQuery : IRequest<QueryFilterResultsViewModel<ProductTypeViewModel>>
{
    public ProductTypesFilterViewModel? Parameters { get; set; }
}



public class GetProductTypesQueryHandler(IDatabaseContext context) : IRequestHandler<GetProductTypesQuery, QueryFilterResultsViewModel<ProductTypeViewModel>>
{
    private readonly IDatabaseContext _context = context;

    public async Task<QueryFilterResultsViewModel<ProductTypeViewModel>> Handle(GetProductTypesQuery req, CancellationToken token = default)
    {
        QueryFilterResultsViewModel<ProductTypeViewModel> getProductTypes()
        {

            var currencies = _context.ProductTypes.ToArray();

            List<ProductTypeViewModel> results = [];
            int resultsCount = 0;


            if (req.Parameters is { })
            {

                // filter by supplied parameters
                if (req.Parameters.Name?.Length > 0)
                {
                    currencies = [.. currencies.Where(currency => currency.Name.Contains(req.Parameters.Name, StringComparison.InvariantCultureIgnoreCase))];
                }


                // apply sort parameters
                var sortDesc = req.Parameters.OrderBy == "DESC";
                currencies = (req.Parameters.SortBy?.ToLower() ?? "") switch
                {
                    _ => sortDesc ? [.. currencies.OrderByDescending(currency => currency.Name)] : [.. currencies.OrderBy(currency => currency.Name)],
                };

                resultsCount = results.Count;

                // then filter according to page size
                if (req.Parameters?.PageSize > 0 && req.Parameters?.PageIndex > 0)
                {

                    results.AddRange(currencies.Skip(
                        req.Parameters.PageSize * (req.Parameters.PageIndex - 1)
                    ).Take(req.Parameters.PageSize).Select(c => c.ToViewModel()));
                }
                else
                {
                    results.AddRange(currencies.Select(c => c.ToViewModel()));
                }
            }
            else
            {
                resultsCount = currencies.Length;

                results.AddRange(currencies.Select(currency => currency.ToViewModel()));
            }

            return new QueryFilterResultsViewModel<ProductTypeViewModel>
            {
                Items = results,
                Total = resultsCount
            };
        }


        return await Task.FromResult(getProductTypes());
    }
}



public class GetProductTypeDetailsQuery : IRequest<DataOperationResult<ProductTypeViewModel>>
{
    public required string ProductTypeName { get; set; }
}



public class GetProductTypeDetailsQueryHandler(IDatabaseContext context) : IRequestHandler<GetProductTypeDetailsQuery, DataOperationResult<ProductTypeViewModel>>
{
    private readonly IDatabaseContext _context = context;

    public async Task<DataOperationResult<ProductTypeViewModel>> Handle(GetProductTypeDetailsQuery request, CancellationToken cancellationToken = default)
    {
        DataOperationResult<ProductTypeViewModel> getProductType()
        {
            var currency = _context.ProductTypes.FirstOrDefault(currency => currency.Name.Contains(request.ProductTypeName, StringComparison.InvariantCultureIgnoreCase)) ?? null;

            if (currency is not null)
            {
                Task.FromResult(DataOperationResult<ProductTypeViewModel>.Success(currency.ToViewModel()));
            }

            return DataOperationResult<ProductTypeViewModel>.NotFound;
        }

        return await Task.FromResult(getProductType());
    }
}

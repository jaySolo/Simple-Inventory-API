using System;
using System.Linq;
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


namespace jsolo.simpleinventory.sys.commands.Currencies;



public class CreateCurrencyCommand : IRequest<DataOperationResult<CurrencyViewModel>>
{
    public required CurrencyViewModel NewCurrency { get; set; }
}



public class CreateCurrencyCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<CreateCurrencyCommand, DataOperationResult<CurrencyViewModel>>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult<CurrencyViewModel>> Handle(CreateCurrencyCommand request, CancellationToken token)
    {
        if (Context.Currencies.Any(currency => currency.Code.Contains(request.NewCurrency.Code, StringComparison.InvariantCultureIgnoreCase)))
        {
            return Task.FromResult(DataOperationResult<CurrencyViewModel>.Exists);
        }

        try
        {
            var currency = new Currency(
                code: request.NewCurrency.Code,
                name: request.NewCurrency.Name,
                symbol: request.NewCurrency.Symbol ?? "",
                description: request.NewCurrency.Description ?? ""
            );

            Context.BeginTransaction()
                .Add(currency)
                .SaveChanges()
                .CloseTransaction();

            return Task.FromResult(DataOperationResult<CurrencyViewModel>.Success(
                currency.ToViewModel()
            ));

        }
        catch (Exception ex)
        {
            Context.RollbackChanges().CloseTransaction();

            return Task.FromResult(DataOperationResult<CurrencyViewModel>.Failure(
                ex.ToString()
            ));
        }
    }
}



public class DeleteCurrencyCommand : IRequest<DataOperationResult>
{
    public required string CurrencyCode { get; set; }
}



public class DeleteCurrencyCommandHandler(
    IDatabaseContext context,
    ICurrentUserService currentUserService,
    IDateTimeService dateTimeService
) : RequestHandlerBase<DeleteCurrencyCommand, DataOperationResult>(context, currentUserService, dateTimeService)
{
    public override Task<DataOperationResult> Handle(DeleteCurrencyCommand request, CancellationToken token)
    {
        var currency = Context.Currencies.FirstOrDefault(currency => currency.Code.Contains(request.CurrencyCode, StringComparison.InvariantCultureIgnoreCase)) ?? null;

        if (currency is null)
        {
            return Task.FromResult(DataOperationResult.NotFound);
        }

        try
        {
            Context.BeginTransaction()
                .Delete(currency)
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

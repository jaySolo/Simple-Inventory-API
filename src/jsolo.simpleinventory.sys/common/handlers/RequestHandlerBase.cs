using MediatR;

using jsolo.simpleinventory.sys.common.interfaces;


namespace jsolo.simpleinventory.sys.common.handlers
{
    public class RequestHandlerBase<TRequest, TDataType> : IRequestHandler<TRequest, TDataType>
        where TRequest : IRequest<TDataType>
        where TDataType : class
    {
        public readonly IDatabaseContext Context;

        public readonly ICurrentUserService CurrentUser;

        public readonly IDateTimeService DateTime;


        public RequestHandlerBase(
            IDatabaseContext context, ICurrentUserService currentUserService, IDateTimeService dateTimeService
        ) {
            this.Context = context;
            this.CurrentUser = currentUserService;
            this.DateTime = dateTimeService;
        }


        public virtual async Task<TDataType> Handle(TRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("You should override this method!");
        }
    }
}

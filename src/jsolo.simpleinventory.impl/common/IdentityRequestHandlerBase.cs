using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using jsolo.simpleinventory.sys.common.interfaces;


namespace jsolo.simpleinventory.impl.common
{
    public class IdentityRequestHandlerBase<TRequest, TDataType> : IRequestHandler<TRequest, TDataType>
        where TRequest : IRequest<TDataType>
        where TDataType : class
    {
        public readonly IIdentityDatabaseContext Context;

        public readonly ICurrentUserService CurrentUser;

        public readonly IDateTimeService DateTime;
        

        public IdentityRequestHandlerBase(
            IIdentityDatabaseContext context, ICurrentUserService currentUserService, IDateTimeService dateTimeService
        ) {
            this.Context = context;
            this.DateTime = dateTimeService;
            this.CurrentUser = currentUserService;
        }

        public virtual Task<TDataType> Handle(TRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("You should override this method!");
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.Extensions.Logging;

using jsolo.simpleinventory.sys.common.interfaces;


namespace jsolo.simpleinventory.sys.common.behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        private readonly ICurrentUserService _currentUserService;

        private readonly IIdentityService _identityService;


        public UnhandledExceptionBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService,
            IIdentityService identityService
        )
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }


        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken token
        )
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                var userId = _currentUserService.UserId;
                string userName = string.Empty;

                if (userId > 0)
                {
                    userName = await _identityService.GetUserNameAsync(userId);
                }

                if (userName.Length > 0)
                {
                    _logger.LogError(
                        ex,
                        "Failed to complete request '{Name}' recieved from user '{@userName}'.",
                        requestName,
                        userName
                    );

                }
                else
                {
                    _logger.LogError(
                        exception: ex,
                        message: "Failed to complete request '{Name}' recieved from anonymous user.",
                        requestName
                    );
                }
                throw;
            }
        }
    }
}

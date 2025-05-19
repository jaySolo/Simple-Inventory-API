using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.Extensions.Logging;

using jsolo.simpleinventory.sys.common.interfaces;


namespace jsolo.simpleinventory.sys.common.behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;

        private readonly ILogger<TRequest> _logger;

        private readonly ICurrentUserService _currentUserService;

        private readonly IIdentityService _identityService;


        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService,
            IIdentityService identityService)
        {
            _timer = new Stopwatch();

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
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                var userId = _currentUserService.UserId;
                var userName = string.Empty;

                if (userId > 0)
                {
                    userName = await _identityService.GetUserNameAsync(userId);
                }

                if (userName.Length > 0)
                {
                    _logger.LogWarning(
                        "The request '{Name}' made by user '{@UserName}' took longer than expected to complete ({ElapsedMilliseconds} ms).",
                        requestName, userName, elapsedMilliseconds
                    );
                }
                else
                {
                    _logger.LogWarning(
                        "The request '{Name}' made by anonymous user took longer than expected to complete ({ElapsedMilliseconds} ms).",
                        requestName, elapsedMilliseconds
                    );
                }

            }

            return response;
        }
    }
}

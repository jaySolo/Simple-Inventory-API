using System.Threading;
using System.Threading.Tasks;

using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

using jsolo.simpleinventory.sys.common.interfaces;


namespace jsolo.simpleinventory.sys.common.behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;

        private readonly ICurrentUserService _currentUserService;

        private readonly IIdentityService _identityService;


        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }


        public async Task Process(TRequest request, CancellationToken cancellationToken)
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
                _logger.LogInformation("Recieved request '{Name}' from user '{@UserName}'.",
                    requestName, userName);
            }
            else
            {
                _logger.LogInformation("Recieved request '{Name}' from anonymous user.",
                    requestName);
            }
        }
    }
}
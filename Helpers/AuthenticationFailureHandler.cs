using JsonPlaceHolderWrapperService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace JsonPlaceHolderWrapperService.Helpers
{
    public class AuthenticationFailureHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ILogger<AuthenticationFailureHandler> _logger;

        public AuthenticationFailureHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                            ILoggerFactory loggerFactory,
                                            UrlEncoder encoder,
                                            ISystemClock clock)
            : base(options, loggerFactory, encoder, clock) 
        {
            this._logger = loggerFactory.CreateLogger<AuthenticationFailureHandler>();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // This will never be called because it's for failure handling only
            const string message = "HandleAuthenticateAsync was called unexpectedly in AuthenticationFailureHandler.";
            _logger.LogWarning(message);
            return Task.FromResult(AuthenticateResult.Fail("Not implemented"));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            const string message = "Authentication challenge triggered: Invalid or missing credentials.";
            _logger.LogWarning(message);

            Response.StatusCode = StatusCodes.Status401Unauthorized;
            Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                StatusCode = 401,
                Message = "Unauthorized - Invalid or missing credentials"
            };

            var json = JsonSerializer.Serialize(errorResponse);
            await Response.WriteAsync(json);
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            const string message = "Authorization forbidden: User does not have permission.";
            _logger.LogWarning(message);

            Response.StatusCode = StatusCodes.Status403Forbidden;
            Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                StatusCode = 403,
                Message = "Forbidden - You do not have permission to access this resource"
            };

            var json = JsonSerializer.Serialize(errorResponse);
            await Response.WriteAsync(json);
        }
    }
}

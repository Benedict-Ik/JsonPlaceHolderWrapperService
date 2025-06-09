using JsonPlaceHolderWrapperService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace JsonPlaceHolderWrapperService.Helpers
{
    public class AuthenticationFailureHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AuthenticationFailureHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                            ILoggerFactory logger,
                                            UrlEncoder encoder,
                                            ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // This will never be called because it's for failure handling only
            return Task.FromResult(AuthenticateResult.Fail("Not implemented"));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
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

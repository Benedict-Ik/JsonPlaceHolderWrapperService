using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace JsonPlaceHolderWrapperService.Helpers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _config;
        private readonly ILogger<BasicAuthenticationHandler> _logger;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration config
            )
            : base(options, loggerFactory, encoder, clock)
        {
            _config = config;
            _logger = loggerFactory.CreateLogger<BasicAuthenticationHandler>();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            _logger.LogInformation("Authenticating request for {Path}", Request.Path);

            // Check if Authorization header is present
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                const string message = "Missing Authorization header.";
                _logger.LogWarning(message);
                return Task.FromResult(AuthenticateResult.Fail(message));
            }

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                if (!authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
                {
                    const string message = "Invalid authentication scheme.";
                    _logger.LogWarning(message);
                    return Task.FromResult(AuthenticateResult.Fail(message));
                }

                var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

                if (credentials.Length != 2)
                {
                    const string message = "Invalid Authorization header format. Expected username:password.";
                    _logger.LogWarning(message);
                    return Task.FromResult(AuthenticateResult.Fail(message));
                }

                var username = credentials[0];
                var password = credentials[1];

                var validUsername = _config["BasicAuth:Username"];
                var validPassword = _config["BasicAuth:Password"];

                if (username == validUsername && password == validPassword)
                {
                    _logger.LogInformation("Authentication successful for user: {Username}", username);

                    var claims = new[] { new Claim(ClaimTypes.Name, username) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                {
                    const string message = "Invalid username or password.";
                    _logger.LogWarning("Authentication failed for user: {Username}", username);
                    return Task.FromResult(AuthenticateResult.Fail(message));
                }
            }
            catch (FormatException ex)
            {
                const string message = "Invalid Base64 string in Authorization header.";
                _logger.LogError(ex, message);
                return Task.FromResult(AuthenticateResult.Fail(message));
            }
            catch (Exception ex)
            {
                const string message = "Unexpected error occurred during Basic Authentication.";
                _logger.LogError(ex, message);
                return Task.FromResult(AuthenticateResult.Fail(message));
            }
            finally
            {

                _logger.LogInformation("Finished authentication check for {Path}", Request.Path);
            }
        }
    }
}

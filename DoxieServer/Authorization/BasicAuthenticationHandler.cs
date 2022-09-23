using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using DoxieServer.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DoxieServer.Authorization;

public sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private IEnvironmentVariables EnvironmentVariables { get; }

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IEnvironmentVariables environmentVariables
    )
        : base(options, logger, encoder, clock)
    {
        this.EnvironmentVariables = environmentVariables;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        this.Response.Headers.Add("WWW-Authenticate", "Basic");

        if (!this.Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
        }

        var authorizationHeader = this.Request.Headers["Authorization"].ToString();
        var authHeaderRegex = new Regex(@"Basic (.*)");

        if (!authHeaderRegex.IsMatch(authorizationHeader))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization code not formatted properly."));
        }

        var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
        var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
        var authUsername = authSplit[0];
        var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

        if (authUsername != this.EnvironmentVariables.Username || authPassword != this.EnvironmentVariables.Password)
        {
            return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));
        }

        var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, "roundthecode");
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, this.Scheme.Name)));
    }
}

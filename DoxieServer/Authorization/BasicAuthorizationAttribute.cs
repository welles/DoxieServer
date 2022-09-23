using Microsoft.AspNetCore.Authorization;

namespace DoxieServer.Authorization;

public sealed class BasicAuthorizationAttribute : AuthorizeAttribute
{
    public BasicAuthorizationAttribute()
    {
        this.Policy = "BasicAuthentication";
    }
}

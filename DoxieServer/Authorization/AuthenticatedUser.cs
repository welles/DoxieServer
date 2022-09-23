using System.Security.Principal;

namespace DoxieServer.Authorization;

public sealed class AuthenticatedUser : IIdentity
{
    public AuthenticatedUser(string authenticationType, bool isAuthenticated, string name)
    {
        this.AuthenticationType = authenticationType;
        this.IsAuthenticated = isAuthenticated;
        this.Name = name;
    }

    public string AuthenticationType { get; }

    public bool IsAuthenticated { get;}

    public string Name { get; }
}

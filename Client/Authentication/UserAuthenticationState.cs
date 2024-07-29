using System.Security.Claims;

namespace Client.Authentication.Models
{
    public class UserAuthenticationState
    {
        public ClientPrincipal? ClientPrincipal { get; set; }
    }

    public static class AuthenticationHelper
    {
        public static ClaimsPrincipal GetClaimsPrincipalFromClientPrincipal(ClientPrincipal? clientPrincipal)
        {
            if (clientPrincipal is null || clientPrincipal.UserRoles is null || clientPrincipal.UserId is null || clientPrincipal.UserDetails is null)
            {
                return new ClaimsPrincipal();
            }

            try
            {
                clientPrincipal.UserRoles = clientPrincipal.UserRoles.Except(new string[] { "anonymous" }, StringComparer.CurrentCultureIgnoreCase);

                if (!clientPrincipal.UserRoles.Any())
                {
                    return new ClaimsPrincipal();
                }

                var identity = new ClaimsIdentity(clientPrincipal.IdentityProvider);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, clientPrincipal.UserId));
                identity.AddClaim(new Claim(ClaimTypes.Name, clientPrincipal.UserDetails));
                identity.AddClaims(clientPrincipal.UserRoles.Select(r => new Claim(ClaimTypes.Role, r)));

                return new ClaimsPrincipal(identity);
            }
            catch
            {
                return new ClaimsPrincipal();
            }
        }
    }

    public class ClientPrincipal
    {
        public string? IdentityProvider { get; set; }
        public string? UserId { get; set; }
        public string? UserDetails { get; set; }
        public IEnumerable<string>? UserRoles { get; set; }
    }
}
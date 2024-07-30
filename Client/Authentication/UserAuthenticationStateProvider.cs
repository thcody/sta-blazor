using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Authentication
{
    public class UserAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _client;
        public UserAuthenticationStateProvider(IWebAssemblyHostEnvironment environment)
        {
            _client = new HttpClient { BaseAddress = new Uri(environment.BaseAddress) };
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var state = await _client.GetFromJsonAsync<UserAuthenticationState>("/.auth/me");
                var clientPrincipal = state?.ClientPrincipal;

                var claimsPrincipal = AuthenticationHelper.GetClaimsPrincipalFromClientPrincipal(clientPrincipal);

                return new AuthenticationState(new ClaimsPrincipal(claimsPrincipal));
            }
            catch (Exception e)
            {
                var claimsPrincipal = AuthenticationHelper.GetClaimsPrincipalFromClientPrincipal(null);
                return new AuthenticationState(new ClaimsPrincipal(claimsPrincipal));
            }
        }
    }
}

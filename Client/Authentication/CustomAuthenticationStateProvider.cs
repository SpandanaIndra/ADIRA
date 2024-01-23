using ADIRA.Client.Extensions;
using ADIRA.Shared.BusinessDataObjects;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ADIRA.Client.Authentication
{
    public class CustomAuthenticationStateProvider:AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await _sessionStorage.ReadItemEncryptedAsync<UserSession>("UserSession");
                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymous));

                }
                var claimsPrinciple = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
               {
                   new Claim(ClaimTypes.Name,userSession.UserName),
                   new Claim(ClaimTypes.Role,userSession.Role)
               }, "JwtAuth"));
                return await Task.FromResult(new AuthenticationState(claimsPrinciple));


            }
            catch (Exception)
            {

                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }
        public async Task UpdateAuthenticationState(UserSession? userSession)
        {
            ClaimsPrincipal claimsPrincipal;
            if (userSession != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
               {
                   new Claim(ClaimTypes.Name,userSession.UserName),
                   new Claim(ClaimTypes.Role,userSession.Role)
               }));
                userSession.ExpiryTimeStamp = DateTime.Now.AddSeconds(userSession.ExpiresIn);
                await _sessionStorage.SaveItemEncryptedAsync("UserSession", userSession);

            }
            else
            {
                claimsPrincipal = _anonymous;
                await _sessionStorage.RemoveItemAsync("UserSession");
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        public async Task<string> GetToken()
        {
            var result = string.Empty;
            try
            {
                var userSession = await _sessionStorage.ReadItemEncryptedAsync<UserSession>("UserSession");
                if (userSession != null && DateTime.Now < userSession.ExpiryTimeStamp)
                    result = userSession.Token;
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}

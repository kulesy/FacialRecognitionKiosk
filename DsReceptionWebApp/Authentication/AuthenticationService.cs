using DsReceptionClassLibrary.Domain.Entities.Authentication;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using DsReceptionClassLibrary.EndPoints.Token;
using DsReceptionClassLibrary.EndPoints.Clients;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using System;

namespace DsReceptionWebApp.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _config;
        private readonly ITokenEndpoint _tokenEndpoint;
        private readonly IClientEndpoint _clientEndpoint;
        private readonly string _authTokenName;

        public AuthenticationService(HttpClient httpClient,
                                     AuthenticationStateProvider authStateProvider,
                                     ILocalStorageService localStorage,
                                     IConfiguration config,
                                     ITokenEndpoint tokenEndpoint,
                                     IClientEndpoint clientEndpoint)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _config = config;
            _tokenEndpoint = tokenEndpoint;
            _clientEndpoint = clientEndpoint;
            _authTokenName = _config["TokenApi:Auth"];
        }

        public async Task<AuthenticatedUserModel> LoginThroughForm(AuthenticationUserModel userForAuthentication)
        {
            try
            {
                var authContent = await _tokenEndpoint.FormCreateTokenAsync(userForAuthentication);

                var result = JsonSerializer.Deserialize<AuthenticatedUserModel>(
                    authContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                await _localStorage.SetItemAsync(_authTokenName, result.Access_Token);

                await ((AuthStateProvider)_authStateProvider).NotifyUserAuthenticationAsync(result.Access_Token);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Access_Token);

                await _clientEndpoint.PostSignIn(userForAuthentication.Email);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AuthenticatedUserModel> LoginThroughFace(Client userForAuthentication)
        {
            
            var authContent = await _tokenEndpoint.FaceCreateTokenAsync(userForAuthentication);

            var result = JsonSerializer.Deserialize<AuthenticatedUserModel>(
                authContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await _localStorage.SetItemAsync(_authTokenName, result.Access_Token);

            await ((AuthStateProvider)_authStateProvider).NotifyUserAuthenticationAsync(result.Access_Token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Access_Token);
            return result;
        }

        public async Task Logout()
        {
            await ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        }
    }
}

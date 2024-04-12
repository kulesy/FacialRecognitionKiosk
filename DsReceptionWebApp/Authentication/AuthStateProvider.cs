using Blazored.LocalStorage;
using DsReceptionClassLibrary.EndPoints.Clients;
using DsReceptionClassLibrary.EndPoints.Speech;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DsReceptionWebApp.Authentication
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _config;
        private readonly IClientEndpoint _clientEndpoint;
        private readonly ISpeechEndpoint _speechEndpoint;
        private readonly AuthenticationState _anonymous;
        private readonly string _authTokenName;

        public AuthStateProvider(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            IConfiguration config,
            IClientEndpoint clientEndpoint,
            ISpeechEndpoint speechEndpoint)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _config = config;
            _anonymous = new AuthenticationState(
                   new ClaimsPrincipal(
                   new ClaimsIdentity()));
            _authTokenName = _config["TokenApi:Auth"];
            _clientEndpoint = clientEndpoint;
            _speechEndpoint = speechEndpoint;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(_authTokenName);

            if (string.IsNullOrWhiteSpace(token))
            {
                return _anonymous;
            }

            var authResult = await NotifyUserAuthenticationAsync(token);

            if (authResult == false)
            {
                return _anonymous;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            return new AuthenticationState(
                   new ClaimsPrincipal(
                   new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
        }

        public async Task<bool> NotifyUserAuthenticationAsync(string token)
        {
            Task<AuthenticationState> authState;
            var client = await _clientEndpoint.GetUser(token);
            if (client is null)
            {
                
                await NotifyUserLogout();
                return false;
            }

            var authenticatedUser = new ClaimsPrincipal(
                                new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
            authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);

            return true;
        }

        public async Task NotifyUserLogout()
        {
            await _localStorage.RemoveItemAsync(_authTokenName);
            var authState = Task.FromResult(_anonymous);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(authState);
        }
    }
}

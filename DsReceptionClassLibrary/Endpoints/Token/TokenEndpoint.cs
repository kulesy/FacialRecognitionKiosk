using DsReceptionClassLibrary.Domain.Entities.Authentication;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.EndPoints.Token
{
    public class TokenEndpoint : ITokenEndpoint
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public TokenEndpoint(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<string> FormCreateTokenAsync(AuthenticationUserModel userForAuthentication)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", userForAuthentication.Email),
                new KeyValuePair<string, string>("password", userForAuthentication.Password),
                new KeyValuePair<string, string>("grant_type", "password"),
            });

            var api = _config["apiConnection"] + _config["TokenApi:GetToken:Form"];
            var apiResult = await _httpClient.PostAsync(api, data);
            var apiContent = await apiResult.Content.ReadAsStringAsync();

            if (apiResult.IsSuccessStatusCode == false)
            {
                var responseMessage = await apiResult.Content.ReadAsStringAsync();
                throw new Exception(responseMessage.Split('$')[1]);
            }

            return apiContent;
        }

        public async Task<string> FaceCreateTokenAsync(Client userForAuthentication)
        {
            var api = _config["apiConnection"] + _config["TokenApi:GetToken:Face"];
            var apiResult = await _httpClient.PostAsJsonAsync(api, userForAuthentication);
            var apiContent = await apiResult.Content.ReadAsStringAsync();

            if (apiResult.IsSuccessStatusCode == false)
            {
                var responseMessage = await apiResult.Content.ReadAsStringAsync();
                throw new Exception(responseMessage.Split('$')[1]);
            }

            return apiContent;
        }
    }
}

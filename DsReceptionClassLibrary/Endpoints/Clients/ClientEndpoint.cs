using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Authentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using DsReceptionClassLibrary.Domain.Entities.Faces;
using System.IO;

namespace DsReceptionClassLibrary.EndPoints.Clients
{
    public class ClientEndpoint : IClientEndpoint
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public ClientEndpoint(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<Client> GetUser(string token)
        {
            var api = _config["apiConnection"] + _config["ClientApi:Client"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var apiResult = await _httpClient.GetAsync(api);
            if (apiResult.IsSuccessStatusCode is false)
            {
                return null;
            }
            var apiContent = await apiResult.Content.ReadAsStreamAsync();
            var resultContent = await JsonSerializer.DeserializeAsync<Client>(
                apiContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return resultContent;
        }


        public async Task<APIResult> RegisterAsync(RegisterUserModel userForRegistering)
        {

            var api = _config["apiConnection"] + _config["ClientApi:Create"];
            var apiResult = await _httpClient.PostAsJsonAsync(api, userForRegistering);
            var apiContent = await apiResult.Content.ReadAsStreamAsync();
            var resultContent = await JsonSerializer.DeserializeAsync<CQRSResponse>(
                apiContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return new() { Messages = resultContent.Messages, StatusCode = apiResult.StatusCode };
        }

        public async Task<List<Login>> GetSignIns(int pageNumber)
        {
            var api = _config["apiConnection"] + _config["ClientApi:SignInsGet"];
            var apiResult = await _httpClient.PostAsJsonAsync(api, pageNumber);
            if (apiResult.IsSuccessStatusCode is false)
            {
                return null;
            }
            var apiContent = await apiResult.Content.ReadAsStreamAsync();
            var resultContent = await JsonSerializer.DeserializeAsync<List<Login>>(
                apiContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return resultContent;
        }

        public async Task PostSignIn(string email)
        {
            var dataRegister = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Email", email)
            });

            var api = _config["apiConnection"] + _config["ClientApi:SignInPost"];
            var apiResult = await _httpClient.PostAsync(api, dataRegister);
            if (apiResult.IsSuccessStatusCode == false)
            {
                var responseMessage = await apiResult.Content.ReadAsStringAsync();
                throw new Exception(responseMessage.Split('$')[1]);
            }
        }

        public async Task<APIResult> PostImageOfClientWithFile(List<IBrowserFile> files, List<int> targetFace = null)
        {
            var url = _config["apiConnection"] + _config["ClientApi:ClientFacesPost"];

            using var requestContent = new MultipartFormDataContent();
            foreach (var imageFile in files)
            {
                using var fileStream = imageFile.OpenReadStream(maxAllowedSize: int.MaxValue);
                requestContent.Add(new StreamContent(fileStream), "files", imageFile.Name);
            }

            if (targetFace != null)
            {
                var targetFaceString = string.Join(' ', targetFace);
                var faceContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("targetFace", targetFaceString)
                });
                requestContent.Add(faceContent);
            };

            using HttpResponseMessage apiResult = await _httpClient.PostAsync(url, requestContent);
            var apiContent = await apiResult.Content.ReadAsStreamAsync();
            var resultContent = await JsonSerializer.DeserializeAsync<CQRSResponse>(
                apiContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return new() { Messages = resultContent.Messages, StatusCode = apiResult.StatusCode };

        }

        public async Task<APIResult> PostImageOfClientWithImage(ImageEncoded image, List<int> targetFace = null)
        {
            var url = _config["apiConnection"] + _config["ClientApi:ClientFacesPost"];
            Stream stream = new MemoryStream();
            var base64String = image.FileStream.Replace("data:image/png;base64,", "");
            stream.Write(Convert.FromBase64String(base64String));
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            using var requestContent = new MultipartFormDataContent();
            requestContent.Add(new StreamContent(stream), "files", image.FileName);

            if (targetFace != null)
            {
                var targetFaceString = string.Join(' ', targetFace);
                var faceContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("targetFace", targetFaceString)
                });
                requestContent.Add(faceContent);
            };

            using HttpResponseMessage apiResult = await _httpClient.PostAsync(url, requestContent);
            var apiContent = await apiResult.Content.ReadAsStreamAsync();
            var resultContent = await JsonSerializer.DeserializeAsync<CQRSResponse>(
                apiContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return new() { Messages = resultContent.Messages, StatusCode = apiResult.StatusCode };

        }

        public async Task<List<ImageEncoded>> GetImagesOfClient(string token)
        {
            var api = _config["apiConnection"] + _config["ClientApi:ClientFacesGet"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var apiResult = await _httpClient.GetAsync(api);
            if (apiResult.IsSuccessStatusCode is false)
            {
                var responseMessage = await apiResult.Content.ReadAsStringAsync();
                throw new Exception(responseMessage.Split('$')[1]);
            }
            var apiContent = await apiResult.Content.ReadAsStreamAsync();
            var resultContent = await JsonSerializer.DeserializeAsync<List<ImageEncoded>>(
                apiContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return resultContent;
        }

        public async Task DeleteImagesOfClient(string token)
        {
            var api = _config["apiConnection"] + _config["ClientApi:ClientFacesDelete"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var apiResult = await _httpClient.DeleteAsync(api);
            var apiContent = await apiResult.Content.ReadAsStreamAsync();
            var resultContent = await JsonSerializer.DeserializeAsync<CQRSResponse>(
                apiContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            if (apiResult.IsSuccessStatusCode == false)
            {
                throw new Exception(resultContent.Messages[0]);
            }
        }
    }
}

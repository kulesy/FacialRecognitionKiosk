using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.EndPoints.Face
{
    public class FaceEndpoint : IFaceEndpoint
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public FaceEndpoint(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<List<DetectedFace>> DetectAllFaces(List<IBrowserFile> files)
        {
            
            var url = _config["apiConnection"] + _config["FaceApi:Detection"];

            using var requestContent = new MultipartFormDataContent();
            foreach (var imageFile in files)
            {
                using var fileStream = imageFile.OpenReadStream(maxAllowedSize: int.MaxValue);
                requestContent.Add(new StreamContent(fileStream), "files", imageFile.Name);
            }

            using HttpResponseMessage apiResult = await _httpClient.PostAsync(url, requestContent);
            if (apiResult.IsSuccessStatusCode is false)
            {
                var responseMessage = await apiResult.Content.ReadAsStringAsync();
                responseMessage = responseMessage.Split('$')[1];
                throw new Exception(responseMessage);
            }
            var apiContent = await apiResult.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<List<DetectedFace>>(
                apiContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
            
        }

        public async Task<List<IdentifyResult>> DetectAllClientFaces(List<DetectedFace> detectedFaces)
        {
            var url = _config["apiConnection"] + _config["FaceApi:ClientFaces"];
            using HttpResponseMessage apiResult = await _httpClient.PostAsJsonAsync(url, detectedFaces);
            if (apiResult.IsSuccessStatusCode is false)
            {
                var responseMessage = await apiResult.Content.ReadAsStringAsync();
                responseMessage = responseMessage.Split('$')[1];
                throw new Exception(responseMessage);
            }
            var apiContent = await apiResult.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<List<IdentifyResult>>(
                apiContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }

        public async Task<List<Client>> DetectAllClients(List<IdentifyResult> identifyResults)
        {
            var url = _config["apiConnection"] + _config["FaceApi:Clients"];
            using HttpResponseMessage apiResult = await _httpClient.PostAsJsonAsync(url, identifyResults);
            if (apiResult.IsSuccessStatusCode is false)
            {
                var responseMessage = await apiResult.Content.ReadAsStringAsync();
                responseMessage = responseMessage.Split('$')[1];
                throw new Exception(responseMessage);
            }
            var apiContent = await apiResult.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<List<Client>>(
                apiContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }

        public async Task<CQRSResponse> TrainFaces()
        {
            var url = _config["apiConnection"] + _config["FaceApi:Train"];

            using var requestContent = new MultipartFormDataContent();
            using HttpResponseMessage apiResult = await _httpClient.PostAsync(url, requestContent);
            var apiContent = await apiResult.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<CQRSResponse>(
                apiContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }
        public List<string> DetectionMessage(List<Client> clientsInImage, int unknownCount = 0)
        {
            List<string> speechMessage = new();
            string welcomeMessage = $"Welcome to Digital Stock";
            string unrecognizedSignIn = "Please register or sign in on the kiosk provided";
            string noRecognition = "There are no detected faces in this image";
            if (clientsInImage.Any() || unknownCount > 0)
            {
                switch (clientsInImage.Count)
                {
                    case 1:
                        welcomeMessage += $" {clientsInImage[0].FirstName}";
                        speechMessage.Add(welcomeMessage);
                        speechMessage.Add("You have been automatically signed in");
                        break;
                    case > 1:
                        welcomeMessage += $", { string.Join(", ", clientsInImage.SkipLast(1).Select(s => s.FirstName))} and {clientsInImage.Last().FirstName}";
                        speechMessage.Add(welcomeMessage);
                        speechMessage.Add("You all have been automatically signed in");
                        break;
                    default:
                        speechMessage.Add(welcomeMessage);
                        break;
                }

                switch (unknownCount)
                {
                    case 1:;
                        speechMessage.Add("There is an unrecognized client");
                        speechMessage.Add(unrecognizedSignIn);
                        break;
                    case > 1:
                        speechMessage.Add($"There are {unknownCount} unrecognized clients");
                        speechMessage.Add(unrecognizedSignIn);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                speechMessage.Add(noRecognition);
            }
            return speechMessage;
        }
    }
}

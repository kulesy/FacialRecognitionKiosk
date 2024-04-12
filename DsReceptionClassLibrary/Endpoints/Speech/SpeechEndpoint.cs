using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.EndPoints.Speech
{
    public class SpeechEndpoint : ISpeechEndpoint
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public SpeechEndpoint(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task TextToSpeech(string text)
        {
            var api = _config["apiConnection"] + _config["SpeechApi:Text"];
            await _httpClient.PostAsJsonAsync(api, text);
        }

    }
}

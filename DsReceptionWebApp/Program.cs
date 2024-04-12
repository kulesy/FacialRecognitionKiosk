using Blazored.LocalStorage;
using DsReceptionClassLibrary.EndPoints.Clients;
using DsReceptionClassLibrary.EndPoints.Face;
using DsReceptionClassLibrary.EndPoints.Speech;
using DsReceptionClassLibrary.EndPoints.Token;
using DsReceptionWebApp.Authentication;
using DsReceptionWebApp.Caches;
using DsReceptionWebApp.Stores.SpeechStore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DsReceptionWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            builder.Services.AddScoped<ITokenEndpoint, TokenEndpoint>();
            builder.Services.AddScoped<IClientEndpoint, ClientEndpoint>();
            builder.Services.AddScoped<IFaceEndpoint, FaceEndpoint>();
            builder.Services.AddScoped<ISpeechEndpoint, SpeechEndpoint>();

            builder.Services.AddScoped<ErrorStore>();
            builder.Services.AddScoped<SpeechStore>();

            builder.Services.AddScoped<IImageCache, ImageCache>();
            builder.Services.AddScoped<IFacesCache, FacesCache>();

            builder.Services.AddScoped(sp => new HttpClient());
            await builder.Build().RunAsync();
        }
    }
}

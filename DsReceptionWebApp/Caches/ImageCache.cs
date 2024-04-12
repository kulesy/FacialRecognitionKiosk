using Blazored.LocalStorage;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Faces;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DsReceptionWebApp.Caches
{
    public class ImageCache : IImageCache
    {
        private readonly ILocalStorageService _localStorage;
        private string faceName = "clientImage";
        private string faceCacheDate = "cacheDate";

        public ImageCache(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<ImageEncoded> GetCachedImageAsync()
        {
            return await _localStorage.GetItemAsync<ImageEncoded>(faceName);
        }

        public async Task<DateTime?> GetCachedDateAsync()
        {
            return await _localStorage.GetItemAsync<DateTime?>(faceCacheDate);
        }
        public async Task CreateCacheAsync(ImageEncoded face)
        {
            await _localStorage.SetItemAsync(faceName, face);
            await _localStorage.SetItemAsync(faceCacheDate, DateTime.UtcNow);
        }

        public async Task DeleteCacheAsync()
        {
            await _localStorage.RemoveItemAsync(faceName);
            await _localStorage.RemoveItemAsync(faceCacheDate);
        }

    }
}

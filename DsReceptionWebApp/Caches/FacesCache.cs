 using Blazored.LocalStorage;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DsReceptionWebApp.Caches
{
    public class FacesCache : IFacesCache
    {
        private readonly ILocalStorageService _localStorage;
        private string faceName = "clientFaces";
        private string faceCacheDate = "cacheDate";

        public FacesCache(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<List<DetectedFace>> GetCachedFacesAsync()
        {
            return await _localStorage.GetItemAsync<List<DetectedFace>>(faceName);
        }

        public async Task<DateTime?> GetCachedDateAsync()
        {
            return await _localStorage.GetItemAsync<DateTime?>(faceCacheDate);
        }
        public async Task CreateCacheAsync(List<DetectedFace> faces)
        {
            await _localStorage.SetItemAsync(faceName, faces);
            await _localStorage.SetItemAsync(faceCacheDate, DateTime.UtcNow);
        }

        public async Task AddFaceToCacheAsync(DetectedFace face)
        {
            var faces = await GetCachedFacesAsync();
            await DeleteCacheAsync();
            faces.Add(face);
            await _localStorage.SetItemAsync(faceName, faces);
            await _localStorage.SetItemAsync(faceCacheDate, DateTime.UtcNow);
        }

        public async Task DeleteFaceFromCacheAsync(DetectedFace face)
        {
            var faces = await GetCachedFacesAsync();
            faces.Remove(face);
            await CreateCacheAsync(faces);
        }

        public async Task DeleteCacheAsync()
        {
            await _localStorage.RemoveItemAsync(faceName);
            await _localStorage.RemoveItemAsync(faceCacheDate);
        }
    }
}

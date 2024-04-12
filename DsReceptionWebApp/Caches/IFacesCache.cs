using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DsReceptionWebApp.Caches
{
    public interface IFacesCache
    {
        Task AddFaceToCacheAsync(DetectedFace face);
        Task CreateCacheAsync(List<DetectedFace> faces);
        Task DeleteCacheAsync();
        Task DeleteFaceFromCacheAsync(DetectedFace face);
        Task<DateTime?> GetCachedDateAsync();
        Task<List<DetectedFace>> GetCachedFacesAsync();
    }
}
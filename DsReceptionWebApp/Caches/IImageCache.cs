using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Faces;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DsReceptionWebApp.Caches
{
    public interface IImageCache
    {
        Task CreateCacheAsync(ImageEncoded face);
        Task DeleteCacheAsync();
        Task<DateTime?> GetCachedDateAsync();
        Task<ImageEncoded> GetCachedImageAsync();
    }
}
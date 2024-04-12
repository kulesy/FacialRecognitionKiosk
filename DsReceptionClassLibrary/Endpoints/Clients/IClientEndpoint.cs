using DsReceptionClassLibrary.Domain.Entities.Authentication;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Faces;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.EndPoints.Clients
{
    public interface IClientEndpoint
    {
        Task DeleteImagesOfClient(string token);
        Task<List<ImageEncoded>> GetImagesOfClient(string token);
        Task<List<Login>> GetSignIns(int pageNumber);
        Task<Client> GetUser(string token);
        Task<APIResult> PostImageOfClientWithFile(List<IBrowserFile> files, List<int> targetFace = null);
        Task<APIResult> PostImageOfClientWithImage(ImageEncoded image, List<int> targetFace = null);
        Task PostSignIn(string clientId);
        Task<APIResult> RegisterAsync(RegisterUserModel userForRegistering);
    }
}
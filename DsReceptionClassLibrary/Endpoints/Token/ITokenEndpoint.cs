using DsReceptionClassLibrary.Domain.Entities.Authentication;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.EndPoints.Token
{
    public interface ITokenEndpoint
    {
        Task<string> FaceCreateTokenAsync(Client userForAuthentication);
        Task<string> FormCreateTokenAsync(AuthenticationUserModel userForAuthentication);
    }
}
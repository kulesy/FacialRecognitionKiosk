using DsReceptionClassLibrary.Domain.Entities.Authentication;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using System.Threading.Tasks;

namespace DsReceptionWebApp.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> LoginThroughFace(Client userForAuthentication);
        Task<AuthenticatedUserModel> LoginThroughForm(AuthenticationUserModel userForAuthentication);
        Task Logout();
    }
}
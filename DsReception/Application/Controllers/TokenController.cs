using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DsReceptionAPI.Application.Token.Commands.CreateToken;
using DsReceptionClassLibrary.Domain.Entities.Clients;

namespace DsReceptionAPI.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ApiControllerBase
    {
        [HttpPost]
        [Route("form")]
        public async Task<IActionResult> FormCreateAsync([FromForm] string username, [FromForm] string password)
        {
            return await Mediator.Send(new FormCreateTokenCommand() { Username = username, Password = password });
        }

        [HttpPost]
        [Route("face")]
        public async Task<IActionResult> FaceCreateAsync([FromBody] Client user)
        {
            return await Mediator.Send(new FaceCreateTokenCommand() { User = user });
        }
    }
}

using DsReceptionAPI.Application.Clients.Commands.CreateClient;
using DsReceptionAPI.Application.Clients.Commands.PostSignIn;
using DsReceptionAPI.Application.Clients.Commands.DeleteClients;
using DsReceptionAPI.Application.Clients.Queries.GetClients;
using DsReceptionAPI.Application.Clients.Queries.GetSignIns;
using DsReceptionAPI.Application.Face.Commands.AddImageForTraining;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Authentication;
using System;

namespace DsReceptionAPI.Application.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ApiControllerBase
    {
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterUserModel client)
        {
            return Ok(await Mediator.Send(new CreateClientCommand() { FirstName = client.FirstName,
                                                                      LastName = client.LastName,
                                                                      EmailAddress = client.Email,
                                                                      Password = client.Password,
                                                                      Company = client.Company,
                                                                      EnabledFaceIdentification = client.EnabledFaceIdentification
            }));
        }




        [Authorize(AuthenticationSchemes = "JwtBearer")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = await Mediator.Send(new GetClientQuery { Id = userId});
            return Ok(client);
        }


        [Route("all")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllClientsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [Route("azure/personGroup")]
        [HttpDelete]
        public async Task<IActionResult> DeletePersonGroup()
        {
            return Ok(await Mediator.Send(new DeletePersonGroupCommand()));
        }

        [Route("signInPost")]
        [HttpPost]
        public async Task<IActionResult> PostSignIn([FromForm] string email)
        {
            return Ok(await Mediator.Send(new PostSignInCommand() { Email = email }));
        }

        [Route("signInsGet")]
        [HttpPost]
        public async Task<IActionResult> GetSignIns([FromBody] int pageNumber)
        {
            var signIns = await Mediator.Send(new GetSignInsQuery() { PageNumber = pageNumber});
            return Ok(signIns.Items);
        }

        [Authorize(AuthenticationSchemes = "JwtBearer")]
        [DisableRequestSizeLimit]
        [Route("facesPost")]
        [HttpPost]
        public async Task<IActionResult> PostImageOfClient([FromForm] List<IFormFile> files, [FromForm] string targetFace = null)
        {
            List<int> intTargetFaceList = new();
            if (targetFace != null)
            {
                string[] targetFaceList = targetFace.Split('+');
                foreach (var val in targetFaceList)
                {
                    var num = val.Replace("targetFace=", "");
                    intTargetFaceList.Add(int.Parse(num));
                }
            }
            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await Mediator.Send(new AddImageForTrainingCommand() { Id = userId, Images = files, targetFace = intTargetFaceList }));
        }

        [Authorize(AuthenticationSchemes = "JwtBearer")]
        [Route("facesGet")]
        [HttpGet]
        public async Task<IActionResult> GetImagesOfClient()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var clientFaces = await Mediator.Send(new GetClientFacesQuery() { Id = userId });
            if (clientFaces is null)
            {
                return BadRequest("User is does not have any faces");
            }
            return Ok(clientFaces);
        }

        [Authorize(AuthenticationSchemes = "JwtBearer")]
        [Route("facesDelete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteImagesOfClient()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await Mediator.Send(new DeleteClientFacesCommand { Id = userId }));
        }
    }
}

using DsReceptionAPI.Application.Face.Commands.GetFacesFromImage;
using DsReceptionAPI.Application.Face.Commands.GetClientsFromIdentifyResult;
using DsReceptionAPI.Application.Face.Services;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using DsReceptionAPI.Application.Face.Commands.GetCandidateIdentifyResult;

namespace DsReceptionAPI.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaceController : ApiControllerBase
    {

        private readonly ILogger<FaceController> _logger;
        private readonly IConfiguration config;
        private readonly FaceService faceService;

        public FaceController(ILogger<FaceController> logger, IConfiguration config, FaceService faceService)
        {
            _logger = logger;
            this.config = config;
            this.faceService = faceService;
        }

        /// <summary>
        /// Gets faces from an image
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<DetectedFace>> DetectFaces([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count() != 1)
            {
                throw new ("$There must be one file.$");
            }

            using var stream = files.First().OpenReadStream();

            var result = await Mediator.Send(new GetFacesFromImageCommand { Image = stream });

            return result;
        }

        [Route("clientFaces")]
        [HttpPost]
        public async Task<List<IdentifyResult>> DetectClients([FromBody] List<DetectedFace> detectedFaces)
        {
            return await Mediator.Send(new GetCandidateIdentifyResultCommand() { DetectedFaces = detectedFaces });
        }

        [Route("clients")]
        [HttpPost]
        public async Task<List<Client>> DetectClients([FromBody] List<IdentifyResult> clientFaces)
        {
            return await Mediator.Send(new GetClientsFromIdentifyResultCommand() {  IdentifyResultCandidates = clientFaces });
        }

        [HttpPost]
        [Route("train")]
        public async Task<CQRSResponse> Train()
        {
            var trainingResult = await faceService.Train();
            if (trainingResult == true)
            {
                return new() { Messages = new() { "Training has succeeded" }, StatusCode = System.Net.HttpStatusCode.OK };
            }
            else if (trainingResult == false)
            {
                return new() { Messages = new() { "Training has failed" }, StatusCode = System.Net.HttpStatusCode.BadRequest };
            }
            return new() { Messages = new() { "An unknown error has occured" }, StatusCode = System.Net.HttpStatusCode.BadRequest };
        }
    }
}

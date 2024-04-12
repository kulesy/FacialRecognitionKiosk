using DsReceptionAPI.Application.Face.Services;
using DsReceptionAPI.Application.Services;
using DsReceptionAPI.Application.Speech.Commands.SpeekText;
using DsReceptionAPI.Infrastructure.Persistence;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Face.Commands.GetFacesFromImage
{
    public record GetFacesFromImageCommand : IRequest<List<DetectedFace>>
    {
        public Stream Image { get; init; }
    }

    public class GetFacesFromImageCommandHandler : IRequestHandler<GetFacesFromImageCommand, List<DetectedFace>>
    {
        private readonly FaceService faceService;
        private readonly IConfiguration config;

        public GetFacesFromImageCommandHandler(FaceService faceService, IConfiguration config)
        {
            this.faceService = faceService;
            this.config = config;
        }

        public async Task<List<DetectedFace>> Handle(GetFacesFromImageCommand request, CancellationToken cancellationToken)
        {
            IFaceClient faceClient = await faceService.GetClient();

            var detectedFaces = await faceClient.Face.DetectWithStreamAsync(request.Image);

            return detectedFaces.ToList();

            
        }
    }
}

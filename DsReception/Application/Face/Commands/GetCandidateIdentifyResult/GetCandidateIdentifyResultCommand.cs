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

namespace DsReceptionAPI.Application.Face.Commands.GetCandidateIdentifyResult
{
    public record GetCandidateIdentifyResultCommand : IRequest<List<IdentifyResult>>
    {
        public List<DetectedFace> DetectedFaces { get; init; }
    }

    public class GetCandidateIdentifyResultCommandHandler : IRequestHandler<GetCandidateIdentifyResultCommand, List<IdentifyResult>>
    {
        private readonly FaceService faceService;
        private readonly IConfiguration config;

        public GetCandidateIdentifyResultCommandHandler(FaceService faceService, IConfiguration config)
        {
            this.faceService = faceService;
            this.config = config;
        }

        public async Task<List<IdentifyResult>> Handle(GetCandidateIdentifyResultCommand request, CancellationToken cancellationToken)
        {
            IFaceClient faceClient = await faceService.GetClient();

            var sourceFaceIds = request.DetectedFaces.Select(m => m.FaceId.Value).ToList();
            if (sourceFaceIds.Any() == false)
            {
                return new();
            }
            try
            {
                var identifyResults = await faceClient.Face.IdentifyAsync(sourceFaceIds, config.GetValue<string>("FaceApi:PersonGroup:Id"));
                var identifyResultsCandidatesOnly = identifyResults.Where(i => i.Candidates.Any());
                return identifyResultsCandidatesOnly.ToList();
            }
            catch (Exception)
            {

                return new();
            }
        }
    }
}


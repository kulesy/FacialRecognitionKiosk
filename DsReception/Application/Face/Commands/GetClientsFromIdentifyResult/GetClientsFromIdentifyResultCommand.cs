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

namespace DsReceptionAPI.Application.Face.Commands.GetClientsFromIdentifyResult
{
    public record GetClientsFromIdentifyResultCommand : IRequest<List<Client>>
    {
        public List<IdentifyResult> IdentifyResultCandidates { get; init; }
    }

    public class GetClientsFromIdentifyResultCommandHandler : IRequestHandler<GetClientsFromIdentifyResultCommand, List<Client>>
    {
        private readonly FaceService faceService;
        private readonly IConfiguration config;
        private readonly ApplicationDbContext db;
        private readonly SpeechService speechService;
        private readonly UserManager<Client> _userManager;

        public GetClientsFromIdentifyResultCommandHandler(FaceService faceService, IConfiguration config, ApplicationDbContext db, SpeechService speechService, UserManager<Client> userManager)
        {
            this.faceService = faceService;
            this.config = config;
            this.db = db;
            this.speechService = speechService;
            _userManager = userManager;
        }

        public async Task<List<Client>> Handle(GetClientsFromIdentifyResultCommand request, CancellationToken cancellationToken)
        {
            IFaceClient faceClient = await faceService.GetClient();
            List<string> clients = new();

            foreach (var identifyResult in request.IdentifyResultCandidates)
            {
                foreach (var candidate in identifyResult.Candidates)
                {
                    try
                    {
                        Person person = await faceClient.PersonGroupPerson.GetAsync(config.GetValue<String>("FaceApi:PersonGroup:Id"), candidate.PersonId);
                        clients.Add(person.Name);
                    }
                    catch (Exception)
                    {

                        throw new("$Detected person is not registered$");
                    }
                    
                }
            }
            List<Client> clientsInImage = _userManager.Users.Where(c => clients.Contains(c.Id)).ToList();
            foreach(var client in clientsInImage)
            {
                db.Logins.Add(new() { Client = client, FullName = client.FirstName + " " + client.LastName, SignInDate = DateTime.Now });
                await db.SaveChangesAsync();
            }
            return clientsInImage;
        }
    }
}


using DsReceptionAPI.Application.Face.Services;
using DsReceptionAPI.Infrastructure.Persistence;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Clients.Commands.DeleteClients
{
    public class DeletePersonGroupCommand : IRequest<CQRSResponse>
    {
    }

    public class DeletePersonGroupCommandHandler : IRequestHandler<DeletePersonGroupCommand, CQRSResponse>
    {
        private readonly FaceService faceService;
        private readonly UserManager<Client> _userManager;
        private readonly IConfiguration config;
        private readonly ApplicationDbContext db;

        public DeletePersonGroupCommandHandler(IConfiguration config, FaceService faceService, UserManager<Client> userManager, ApplicationDbContext db)
        {
            this.config = config;
            this.faceService = faceService;
            _userManager = userManager;
            this.db = db;
        }

        public async Task<CQRSResponse> Handle(DeletePersonGroupCommand request, CancellationToken cancellationToken)
        {
            IFaceClient faceClient = await faceService.GetClient();
            var clients = _userManager.Users.Include(e => e.Images);
            foreach (var client in clients)
            {
                client.PersonId = null;
                client.Images.Clear();
            }
            await db.SaveChangesAsync();
            await faceClient.PersonGroup.DeleteAsync(config.GetValue<string>("FaceApi:PersonGroup:Id"));
            return new();
        }
    }

    
}

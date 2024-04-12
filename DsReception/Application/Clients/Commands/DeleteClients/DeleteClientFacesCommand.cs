using DsReceptionAPI.Application.Face.Services;
using DsReceptionAPI.Infrastructure.Persistence;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Clients.Commands.DeleteClients
{
    public class DeleteClientFacesCommand : IRequest<CQRSResponse>
    {
        public string Id { get; set; }
    }

    public class DeleteClientFacesCommandValidator : AbstractValidator<DeleteClientFacesCommand>
    {
        public DeleteClientFacesCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    public class DeleteClientFacesCommandHandler : IRequestHandler<DeleteClientFacesCommand, CQRSResponse>
    {
        private readonly FaceService faceService;
        private readonly UserManager<Client> _userManager;
        private readonly IConfiguration config;
        private readonly ApplicationDbContext db;

        public DeleteClientFacesCommandHandler(IConfiguration config, FaceService faceService, UserManager<Client> userManager, ApplicationDbContext db)
        {
            this.config = config;
            this.faceService = faceService;
            _userManager = userManager;
            this.db = db;
        }

        public async Task<CQRSResponse> Handle(DeleteClientFacesCommand request, CancellationToken cancellationToken)
        {
            IFaceClient faceClient = await faceService.GetClient();
            var client = await _userManager.Users.Include(e => e.Images).FirstOrDefaultAsync(f => f.Id == request.Id);
            await faceClient.PersonGroupPerson.DeleteAsync(config.GetValue<string>("FaceApi:PersonGroup:Id"), client.PersonId.Value);
            client.PersonId = null;
            client.Images.Clear();
            await db.SaveChangesAsync();
            return new();
        }
    }

    
}

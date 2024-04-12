using DsReceptionAPI.Application.Face.Services;
using DsReceptionAPI.Infrastructure.Persistence;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Face.Commands.AddImageForTraining
{
    /// <summary>
    /// Takes a client Id and a image to associate with the client.
    /// </summary>
    public record AddImageForTrainingCommand : IRequest<CQRSResponse>
    {
        public string Id { get; set; }

        public List<IFormFile> Images { get; set; }
        // left, top, width, height
        public List<int> targetFace { get; set; }
    }

    public class AddImageForTrainingCommandValidator : AbstractValidator<AddImageForTrainingCommand>
    {
        public AddImageForTrainingCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.Images)
                .NotEmpty();
        }
    }

    public class AddImageForTrainingCommandHandler : IRequestHandler<AddImageForTrainingCommand, CQRSResponse>
    {
        private readonly FaceService faceService;
        private readonly ApplicationDbContext db;
        private readonly IConfiguration config;
        private readonly UserManager<Client> _userManager;

        public AddImageForTrainingCommandHandler(FaceService faceService, ApplicationDbContext db, IConfiguration config, UserManager<Client> userManager)
        {
            this.faceService = faceService;
            this.db = db;
            this.config = config;
            _userManager = userManager;
        }

        public async Task<CQRSResponse> Handle(AddImageForTrainingCommand request, CancellationToken cancellationToken)
        {
            var client = _userManager.Users.Include(e => e.Images).Where(e => e.Id == request.Id).FirstOrDefault();
            if (client.Images.Count < 6)
            {
                IFaceClient faceClient = await faceService.GetClient();

                if (!client.PersonId.HasValue)
                {
                    Person createPersonResult = await faceService.CreatePersonAsync(client.Id);
                    client.PersonId = createPersonResult.PersonId;
                }

                foreach (var file in request.Images)
                {
                        try
                        {
                            using var stream = file.OpenReadStream();
                            HttpOperationResponse<PersistedFace> result = await faceClient.PersonGroupPerson.AddFaceFromStreamWithHttpMessagesAsync(config.GetValue<string>("FaceApi:PersonGroup:Id"), client.PersonId.Value, stream, targetFace: request.targetFace);

                            using var imageStream = file.OpenReadStream();
                            var data = imageStream.Length.ToString();
                            var buffer = new byte[file.Length];
                            await imageStream.ReadAsync(buffer);
                            Image newImage = new() { FileName = file.FileName, FileStream = buffer, Client = client };
                            db.Images.Add(newImage);
                            await db.SaveChangesAsync();
                        }
                        catch (Exception e)
                        {
                        return new()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Messages = new() { "An error has occured, ensure the image only contains one face" }
                        };
                    }
                }
                return new(); 
            }
            else
            {
                return new() { 
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Messages = new() { "You cannot exceed six faces for identification." } };
            }
        }
    }
}

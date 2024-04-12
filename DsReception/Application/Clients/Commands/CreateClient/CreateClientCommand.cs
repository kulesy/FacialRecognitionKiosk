using DsReceptionAPI.Application.Face.Services;
using DsReceptionAPI.Infrastructure.Persistence;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Clients.Commands.CreateClient
{
    public record CreateClientCommand : IRequest<CQRSResponse>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string EmailAddress { get; init; }
        public string Password { get; init; }
        public string Company { get; init; }
        public bool EnabledFaceIdentification { get; init; }
    }

    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty();
            RuleFor(x => x.LastName)
                .NotEmpty();
            RuleFor(x => x.EmailAddress)
                .NotEmpty();
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, CQRSResponse>
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<Client> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateClientCommandHandler(ApplicationDbContext db,
                                          UserManager<Client> userManager,
                                          RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<CQRSResponse> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var company = db.Companys.FirstOrDefault(m => m.Name == request.Company);

            if (company == null)
            {
                company = new Company { Name = request.Company };     
            }

            var client = new Client { 
                UserName = request.EmailAddress, 
                FirstName = request.FirstName, 
                LastName = request.LastName,
                Email = request.EmailAddress,
                Company = company,
                EnabledFaceIdentification = request.EnabledFaceIdentification
            };

            var existingUser = await _userManager.FindByEmailAsync(request.EmailAddress);
            if (existingUser is null)
            {
                IdentityResult createResult = await _userManager.CreateAsync(client, request.Password);
                if (createResult.Succeeded)
                {
                    existingUser = await _userManager.FindByEmailAsync(request.EmailAddress);
                    if (existingUser is null)
                    {
                        return new()
                        {
                            StatusCode = System.Net.HttpStatusCode.InternalServerError,
                            Messages = new() { "An error has occured within the database" }
                        };
                    }
                    IdentityRole roleFind = await _roleManager.FindByNameAsync("User");
                    if (roleFind == null)
                    {
                        var userRole = new IdentityRole { Name = "User", NormalizedName = "User", Id = Guid.NewGuid().ToString() };
                        await _roleManager.CreateAsync(userRole);
                    }
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(existingUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return new();
                    }
                }
                return new()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Messages = createResult.Errors.Select(e => e.Description).ToList()
                };
            }
            return new()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Messages = new() { "That Email already exists, please choose a different email." }
            };
        }
    }
}

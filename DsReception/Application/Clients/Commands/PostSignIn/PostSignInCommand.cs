using DsReceptionAPI.Infrastructure.Persistence;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Clients.Commands.PostSignIn
{
    public record PostSignInCommand : IRequest<CQRSResponse>
    {
        public string Email { get; init; }
    }

    public class PostSignInCommandValidator : AbstractValidator<PostSignInCommand>
    {
        public PostSignInCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty();
        }
    }

    public class PostSignInCommandHandler : IRequestHandler<PostSignInCommand, CQRSResponse>
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<Client> _userManager;

        public PostSignInCommandHandler(ApplicationDbContext db,
                                            UserManager<Client> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }

        public async Task<CQRSResponse> Handle(PostSignInCommand request, CancellationToken cancellationToken)
        {
            var client = await _userManager.FindByEmailAsync(request.Email);
            db.Logins.Add(new() { FullName = client.FirstName + " " + client.LastName, SignInDate = DateTime.Now, Client = client });
            await db.SaveChangesAsync();
            return new();
        }
    }
}

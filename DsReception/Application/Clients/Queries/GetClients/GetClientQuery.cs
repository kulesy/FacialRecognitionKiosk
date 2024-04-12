using AutoMapper;
using AutoMapper.QueryableExtensions;
using DsReceptionAPI.Infrastructure.Persistence;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Clients.Queries.GetClients
{
    public class GetClientQuery : IRequest<Client>
    {
        public string Id { get; set; }
    }
    public class GetClientQueryHandler : IRequestHandler<GetClientQuery, Client>
    {
        private readonly UserManager<Client> _userManager;

        public GetClientQueryHandler(UserManager<Client> userManager)
        {
            _userManager = userManager;
        }

        public Task<Client> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            return _userManager.FindByIdAsync(request.Id);
        }
    }
}

using AutoMapper;
using DsReceptionAPI.Application.Common.BaseQueries;
using DsReceptionAPI.Application.Common.Mappings;
using DsReceptionAPI.Application.Common.Models;
using DsReceptionAPI.Infrastructure.Persistence;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Clients.Queries.GetSignIns
{
    public record GetSignInsQuery : PaginatedQueryBase, IRequest<PaginatedList<Login>>
    {
    }

    public class GetSignInsQueryHandler : IRequestHandler<GetSignInsQuery, PaginatedList<Login>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<Client> _userManager;
        private readonly ApplicationDbContext _db;

        public GetSignInsQueryHandler(IMapper mapper, UserManager<Client> userManager, ApplicationDbContext db)
        {
            _mapper = mapper;
            _userManager = userManager;
            _db = db;
        }

        public Task<PaginatedList<Login>> Handle(GetSignInsQuery request, CancellationToken cancellationToken)
        {
            var logins = _db.Logins
                .OrderByDescending(m => m.SignInDate).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return logins;
        }
    }
}

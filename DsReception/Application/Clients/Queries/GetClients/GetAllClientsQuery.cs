using AutoMapper;
using AutoMapper.QueryableExtensions;
using DsReceptionAPI.Application.Common.BaseQueries;
using DsReceptionAPI.Application.Common.Mappings;
using DsReceptionAPI.Application.Common.Models;
using DsReceptionAPI.Infrastructure.Persistence;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Clients.Queries.GetClients
{
    public record GetAllClientsQuery : PaginatedQueryBase, IRequest<PaginatedList<GetClientsDto>>
    {
    }

    public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, PaginatedList<GetClientsDto>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<Client> _userManager;

        public GetAllClientsQueryHandler(IMapper mapper, UserManager<Client> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public Task<PaginatedList<GetClientsDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            return _userManager.Users
                .OrderBy(m => m.Id == null).ThenBy(m => m.FirstName).ThenBy(m => m.LastName)
                .ProjectTo<GetClientsDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}

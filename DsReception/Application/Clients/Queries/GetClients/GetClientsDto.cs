using AutoMapper;
using DsReceptionAPI.Application.Common.Mappings;
using DsReceptionClassLibrary.Domain.Entities.Clients;

namespace DsReceptionAPI.Application.Clients.Queries.GetClients
{
    public class GetClientsDto : IMapFrom<Client>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }


        public GetClientsCompanyDto Company { get; set; }

        public int ImagesSubmitted { get; set; } = 0;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Client, GetClientsDto>()
                .ForMember(f => f.ImagesSubmitted, opt => opt.MapFrom(m => m.Images.Count));

        }
    }
}

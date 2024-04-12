using DsReceptionAPI.Application.Common.Mappings;
using DsReceptionClassLibrary.Domain.Entities.Clients;

namespace DsReceptionAPI.Application.Clients.Queries.GetClients
{
    public class GetClientsCompanyDto : IMapFrom<Company>
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
    }
}

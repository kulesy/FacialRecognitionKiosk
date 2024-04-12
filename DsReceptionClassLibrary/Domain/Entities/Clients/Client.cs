using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DsReceptionClassLibrary.Domain.Entities.Clients
{
    public class Client : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public Guid? PersonId { get; set; }
        public bool EnabledFaceIdentification { get; set; }
        public IList<Image> Images { get; set; }
    }
}

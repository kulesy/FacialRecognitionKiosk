using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.Domain.Entities.Validation
{
    public class APIResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Messages { get; set; }
    }
}

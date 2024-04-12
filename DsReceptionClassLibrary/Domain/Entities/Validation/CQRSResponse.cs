using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.Domain.Entities.Validation
{
    public record CQRSResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public List<string> Messages { get; set; } = new List<string>() { "Success" };
    }
}

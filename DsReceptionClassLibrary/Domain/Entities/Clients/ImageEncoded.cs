using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.Domain.Entities.Clients
{
    public class ImageEncoded
    {
        public string ClientId { get; set; }
        public string FileStream { get; set; } = "";
        public string FileName { get; set; }
    }
}

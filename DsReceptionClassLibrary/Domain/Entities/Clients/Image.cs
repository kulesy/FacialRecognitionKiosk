using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.Domain.Entities.Clients
{
    public class Image
    {
        public int ImageId { get; set; }
        public byte[] FileStream { get; set; }
        public string FileName { get; set; }
        public string ClientId { get; set; }
        public Client Client { get; set; }
    }
}

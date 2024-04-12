using DsReceptionClassLibrary.Domain.Entities.Clients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.Domain.Entities.Faces
{
    public class FaceImageModel : ImageEncoded
    {
        public Stream ImageStream { get; set; }
    }
}

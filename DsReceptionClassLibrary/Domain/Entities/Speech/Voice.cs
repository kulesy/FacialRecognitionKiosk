using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.Domain.Entities.Speech
{
    public class Voice
    {
        public string Language { get; set; }

        public string Locale { get; set; }

        public string Gender { get; set; }

        public string Name { get; set; }

        public string VoiceForSynthesizer
        {
            get 
            {
                return $"{Locale}-{Name}Neural";
            }
        }

    }
}

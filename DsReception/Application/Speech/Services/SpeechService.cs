using DsReceptionAPI.Application.Speech.Commands.SpeekSSML;
using DsReceptionAPI.Application.Speech.Commands.SpeekText;
using DsReceptionClassLibrary.Domain.Entities.Speech;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Services
{
    public class SpeechService
    {
        private readonly IConfiguration _config;

        public SpeechService(IConfiguration config)
        {
            this._config = config;
        }
        public async Task SynthesizeToSpeakerAsync(SpeekTextCommand command, CancellationToken token)
        {
            //Find your key and resource region under the 'Keys and Endpoint' tab in your Speech resource in Azure Portal
            var config = SpeechConfig.FromSubscription(_config.GetValue<string>("SpeechApi:Key"), _config.GetValue<string>("SpeechApi:Region"));


            var voice = GetVoices.FirstOrDefault(m => m.Name == command.VoiceOf);
            if (voice == null)
                voice = GetVoices.FirstOrDefault(m => m.Name == "Mitchell");

            config.SpeechSynthesisVoiceName = voice.VoiceForSynthesizer;

            SpeechSynthesizer synth = new(config);

            await synth.SpeakTextAsync(command.TextToSpeak);  
        }


        public async Task SynthesizeSSMLToSpeakerAsync(SpeekSSMLCommand command, CancellationToken token)
        {
            //Find your key and resource region under the 'Keys and Endpoint' tab in your Speech resource in Azure Portal
            var config = SpeechConfig.FromSubscription(_config.GetValue<string>("SpeechApi:Key"), _config.GetValue<string>("SpeechApi:Region"));

            SpeechSynthesizer synth = new(config);

            await synth.SpeakSsmlAsync(command.SSMLToSpeak);
        }

        // This should prolly be pulled 
        public static List<Voice> GetVoices = new()
        {
            new Voice{ Language= "English (Australia)", Locale ="en-AU", Gender = "Female", Name = "Natasha"  },
            new Voice{ Language= "English (Australia)", Locale ="en-AU", Gender = "Male", Name = "William"  },
            new Voice{ Language= "English (Canada)", Locale ="en-CA", Gender = "Female", Name = "Clara"  },
            new Voice{ Language= "English (Canada)", Locale ="en-CA", Gender = "Male", Name = "Liam"  },
            new Voice{ Language= "English (Hongkong)", Locale ="en-HK", Gender = "Female", Name = "Yan"  },
            new Voice{ Language= "English (Hongkong)", Locale ="en-HK", Gender = "Male", Name = "Sam"  },
            new Voice{ Language= "English (India)", Locale ="en-IN", Gender = "Female", Name = "Neerja"  },
            new Voice{ Language= "English (India)", Locale ="en-IN", Gender = "Male", Name = "Prabhat"  },
            new Voice{ Language= "English (Ireland)", Locale ="en-IE", Gender = "Female", Name = "Emily"  },
            new Voice{ Language= "English (Ireland)", Locale ="en-IE", Gender = "Male", Name = "Connor"  },
            new Voice{ Language= "English (New Zealand)", Locale ="en-NZ", Gender = "Female", Name = "Molly"  },
            new Voice{ Language= "English (New Zealand)", Locale ="en-NZ", Gender = "Male", Name = "Mitchell"  }
        };
 
    }
}

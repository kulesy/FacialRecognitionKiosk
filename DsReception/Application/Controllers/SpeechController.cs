using DsReceptionAPI.Application.Speech.Commands.SpeekSSML;
using DsReceptionAPI.Application.Speech.Commands.SpeekText;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechController : ApiControllerBase
    {
        [HttpPost]
        [Route("text")]
        public async Task<IActionResult> SpeekText([FromBody] string message)
        {
            return Ok(await Mediator.Send(new SpeekTextCommand() { TextToSpeak = message }));
        }

        [HttpPost]
        [Route("ssml")]
        public async Task<IActionResult> SpeekSSML([FromBody] SpeekSSMLCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}

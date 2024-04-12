using DsReceptionAPI.Application.Services;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Speech.Commands.SpeekText
{
    public record SpeekTextCommand : IRequest<CQRSResponse>
    {
        public string TextToSpeak { get; init; }

        public string VoiceOf { get; init; } = "Mitchell";
    }

    public class SpeekTextCommandValidator : AbstractValidator<SpeekTextCommand>
    {
        public SpeekTextCommandValidator()
        {
            RuleFor(x => x.TextToSpeak)
                .NotEmpty();
            RuleFor(x => x.VoiceOf)
                .NotEmpty();
        }
    }

    public class SpeekTextCommandHandler : IRequestHandler<SpeekTextCommand, CQRSResponse>
    {
        private readonly SpeechService speechService;

        public SpeekTextCommandHandler(SpeechService speechService)
        {
            this.speechService = speechService;
        }

        public async Task<CQRSResponse> Handle(SpeekTextCommand request, CancellationToken cancellationToken)
        {
            await speechService.SynthesizeToSpeakerAsync(request, cancellationToken);

            return new();
        }
    }



}

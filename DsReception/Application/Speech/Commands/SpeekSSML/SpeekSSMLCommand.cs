using DsReceptionAPI.Application.Services;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Speech.Commands.SpeekSSML
{
    public record SpeekSSMLCommand : IRequest<CQRSResponse>
    {
        public string SSMLToSpeak { get; init; }
    }

    public class SpeekSSMLCommandValidator : AbstractValidator<SpeekSSMLCommand>
    {
        public SpeekSSMLCommandValidator()
        {
            RuleFor(x => x.SSMLToSpeak)
                .NotEmpty();
        }
    }

    public class SpeekSSMLCommandHandler : IRequestHandler<SpeekSSMLCommand, CQRSResponse>
    {
        private readonly SpeechService speechService;

        public SpeekSSMLCommandHandler(SpeechService speechService)
        {
            this.speechService = speechService;
        }

        public async Task<CQRSResponse> Handle(SpeekSSMLCommand request, CancellationToken cancellationToken)
        {
            await speechService.SynthesizeSSMLToSpeakerAsync(request, cancellationToken);

            return new();
        }
    }



}

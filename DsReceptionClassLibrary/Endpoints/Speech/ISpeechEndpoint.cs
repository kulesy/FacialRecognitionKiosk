using System.Threading.Tasks;

namespace DsReceptionClassLibrary.EndPoints.Speech
{
    public interface ISpeechEndpoint
    {
        Task TextToSpeech(string text);
    }
}
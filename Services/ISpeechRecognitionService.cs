using IndigoLabsAssigment.Model;

namespace IndigoLabsAssigment.Services
{
    public interface ISpeechRecognitionService
    {
        Task<SpeechRecognitionResponse> RecognizeSpeechAsync(Stream audioStream, string fileName);
    }
}

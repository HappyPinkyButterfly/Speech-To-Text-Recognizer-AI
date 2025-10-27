using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using IndigoLabsAssigment.Model;
using IndigoLabsAssigment.Services;

namespace IndigoLabsAssignment.Services
{
    public class AzureSpeechRecognitionService : ISpeechRecognitionService
    {
        // SpeechConfig is used for connection on azure clound and contains API key, region and settings 
        private readonly SpeechConfig _speechConfig;

        public AzureSpeechRecognitionService(IConfiguration configuration)
        {

            var speechKey = configuration["AzureSpeech:Key"];
            var speechRegion = configuration["AzureSpeech:Region"];

            // creates Azure Speech Config using subscription key and region
            _speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
        }

        public async Task<SpeechRecognitionResponse> RecognizeSpeechAsync(Stream audioStream, string fileName)
        {
            string tempFilePath = null;

            try
            {
                // creates temp file and its path
                tempFilePath = Path.GetTempFileName() + ".wav";

                using (var fileStream = File.Create(tempFilePath))
                {
                    await audioStream.CopyToAsync(fileStream);
                }

                var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new string[] { "en-US", "sl-SI", "de-DE", "fr-FR" });
                using var audioConfig = AudioConfig.FromWavFileInput(tempFilePath); // bridge between azure and the file datoteko
                using var recognizer = new SpeechRecognizer(_speechConfig, autoDetectSourceLanguageConfig, audioConfig);

                // takes audio from audioConfig, sends it to azure cloud, azure ai recognizes
                // the speech and turns it to text, return text and language of it
                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    // detects found language
                    var autoDetectSourceLanguageResult = AutoDetectSourceLanguageResult.FromResult(result);
                    var detectedLanguage = autoDetectSourceLanguageResult.Language;

                    return new SpeechRecognitionResponse
                    {
                        Success = true,
                        Text = result.Text,
                        Language = detectedLanguage
                    };
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    return new SpeechRecognitionResponse
                    {
                        Success = false,
                        Error = "Speech could not be recognized",
                        Language = "unknown"
                    };
                }
                else
                {
                    return new SpeechRecognitionResponse
                    {
                        Success = false,
                        Error = $"Recognition failed: {result.Reason}",
                        Language = "unknown"
                    };
                }
            }
            catch (Exception ex)
            {
                return new SpeechRecognitionResponse
                {
                    Success = false,
                    Error = $"Error: {ex.Message}",
                    Language = "unknown"
                };
            }
            finally // deletes the file at the end
            {
                if (tempFilePath != null && File.Exists(tempFilePath))
                {
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch
                    {
                        // it will always return 200, just
                        // the file might not get deleted
                    }
                }
            }
        }
    }
}
namespace IndigoLabsAssigment.Model
{
    public class SpeechRecognitionResponse
    {
        public string Language { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}

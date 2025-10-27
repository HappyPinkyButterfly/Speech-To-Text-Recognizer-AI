using Microsoft.AspNetCore.Mvc;
using IndigoLabsAssigment.Services;

namespace IndigoLabsAssigment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpeechController : ControllerBase // with controllbase you get access to all HTTP methods
    {
        private readonly ISpeechRecognitionService _speechService;
        private readonly ILogger<SpeechController> _logger;

        public SpeechController(ISpeechRecognitionService speechService, ILogger<SpeechController> logger)
        {
            _speechService = speechService;
            _logger = logger;
        }

        [HttpPost("recognize")]
        [RequestSizeLimit(25 * 1024 * 1024)] // 25MB limit
        // the method will return HTTP to the user
        public async Task<IActionResult> RecognizeSpeech(IFormFile file)
        {
            try
            {
                _logger.LogInformation("Processing speech recognition request");

                // chekcs if file is present
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { error = "No file uploaded" });
                }

                // checks if it is wav file
                if (!file.FileName.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { error = "Only WAV files are supported" });
                }

                // checks size
                if (file.Length > 25 * 1024 * 1024)
                {
                    return BadRequest(new { error = "File size exceeds 25MB limit" });
                }

                // precesses file
                using var stream = file.OpenReadStream();
                var result = await _speechService.RecognizeSpeechAsync(stream, file.FileName);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during speech recognition");
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
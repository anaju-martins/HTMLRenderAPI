using Microsoft.AspNetCore.Mvc;
using HtmlRenderAPI.Services; 

namespace HtmlRenderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RenderController : ControllerBase
    {
        private readonly HtmlRenderService _htmlRenderService;
        private readonly ILogger<RenderController> _logger;

        public RenderController(HtmlRenderService htmlRenderService, ILogger<RenderController> logger)
        {
            _htmlRenderService = htmlRenderService;
            _logger = logger;
        }

        [HttpPost] 
        public async Task<IActionResult> RenderHtml([FromBody] RenderRequest request)
        {
            if (string.IsNullOrEmpty(request.Html))
            {
                return BadRequest("HTML content cannot be empty.");
            }

            try
            {
                string base64Output = await _htmlRenderService.ConvertHtmlToFormattedOutput(request.Html, request.Format);
                return Ok(new { Base64Output = base64Output });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rendering HTML.");
                return StatusCode(500, "An error occurred during HTML rendering.");
            }
        }
    }

    public class RenderRequest
    {
        public string Html { get; set; }
        public OutputFormat Format { get; set; } = OutputFormat.Jpeg; 
    }
}
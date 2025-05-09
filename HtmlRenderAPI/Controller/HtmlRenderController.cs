using HtmlRenderAPI.Services;
using HtmlRenderAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HtmlRenderAPI.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class HtmlRenderController : ControllerBase
    {
        private readonly HtmlRenderService _service; //injetando classe service 

        public HtmlRenderController(HtmlRenderService service)
        {
            _service = service;  //armazenando o que foi injetado para usando no post abaixo
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HtmlRequestModel req)
        {
            if (string.IsNullOrWhiteSpace(req.HtmlContent))
                return BadRequest("HTML content is required.");

            var base64Image = await _service.ConvertHtmlToImage(req.HtmlContent); //chamando a função que foi criada no service
            return Ok(new {ImageBase64 = base64Image}); //montando resposta json com o base64 gerado na função
        }
    }

}

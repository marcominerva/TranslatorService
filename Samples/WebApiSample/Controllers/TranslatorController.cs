using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using TranslatorService;
using TranslatorService.Models.Translation;
using TranslatorService.Settings;
using WebApiSample.Models;

namespace WebApiSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslatorController : ControllerBase
    {
        private readonly ITranslatorClient translatorClient;

        public TranslatorController(ITranslatorClient translatorClient, IOptions<TranslatorSettings> translatorSettingsOptions)
        {
            this.translatorClient = translatorClient;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TranslationResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Translate(TranslatorRequest request)
        {
            var response = await translatorClient.TranslateAsync(request.Text, request.TargetLanguage);
            return Ok(response);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebAPIKurs.Configurations;

namespace WebAPIKurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigurationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpGet("/ConfigSample1")]
        public ContentResult GetSettings()
        {
            string myKeyValue = _configuration["MyKey"];
            string title = _configuration["Position:Title"];
            string name = _configuration["Position:Name"];
            string defaultLogging = _configuration["Logging:LogLevel:Default"];

            return Content($"{myKeyValue} - {title} - {name} - {defaultLogging}");
        }


        [HttpGet("/ConfigSample2")]
        public ContentResult GetGameSettings([FromServices] IOptionsSnapshot<GameSettings> gameSettings)
        {
            GameSettings settings = gameSettings.Value;

            return Content($"{settings.Title} - {settings.SubTitle}");
        }


    }
}

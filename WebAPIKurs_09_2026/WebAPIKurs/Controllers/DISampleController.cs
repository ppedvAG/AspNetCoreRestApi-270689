using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIKurs.DTOs;
using WebAPIKurs.Services;

namespace WebAPIKurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DISampleController : ControllerBase
    {
        private IDateTimeService _dateTimeService;

        private ISingletonGuidService _singletonGuidService;
        private IScopedGuidService _scopedGuidService;
        private ITransientGuidService _transientGuidService;

        //snippet ctor + tab = Konstruktor

        //Dependency Injection (Klasse wird von aussen mit Instanzen befüllt
        //Befüllt wird die Klasse via IOC-Container


        //Ergebnis1 wird via DI-Konstruktor gefüllt
        public DISampleController(IDateTimeService dateTimeService, ISingletonGuidService singletonGuidService, IScopedGuidService scopedGuidService, ITransientGuidService transientGuidService)
        {
            _dateTimeService = dateTimeService;

            _singletonGuidService = singletonGuidService;
            _scopedGuidService = scopedGuidService;
            _transientGuidService = transientGuidService;
        }


        [HttpGet("Locator Anti Pattern")]
        public string AntiPatternSample()
        {
            //Service Locator - Anti Pattern (Feste Kopplung) 
            ITransientGuidService service = this.ControllerContext.HttpContext.RequestServices.GetRequiredService<ITransientGuidService>();

            return "test";
        } 


        [HttpGet]
        public string GetDateTime()
        {
            return _dateTimeService.GetCurrentConstructionTime();
        }

        [HttpGet("/LifecycleSample")]
        public GuidServiceResponseDTO GetDISample([FromServices] ISingletonGuidService singletonGuidService2, IScopedGuidService scopedGuidService2, ITransientGuidService transientGuidService2)
        {
            GuidServiceResponseDTO result = new();

            result.SingletonResult1 = _singletonGuidService.GetGuid();
            result.SingletonResult2 = singletonGuidService2.GetGuid();

            result.ScopedResult1 = _scopedGuidService.GetGuid();
            result.ScopedResult2 = scopedGuidService2.GetGuid();
            
            result.Transient1 = _transientGuidService.GetGuid();
            result.Transient2 = transientGuidService2.GetGuid();


            return result;
        }


        [HttpGet("/LifecycleSample2")]
        public string GetDIWithKeysSample([FromKeyedServices("Singleton")] IGuidService singletonGuidService,
                                          [FromKeyedServices("Scoped")] IGuidService scopeGuidService,
                                          [FromKeyedServices("Transient")] IGuidService transientGuidService) => singletonGuidService.GetGuid() + " - " + scopeGuidService.GetGuid() + " - " + transientGuidService.GetGuid();
        
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Validation;
using WebAPIKurs.Models;
using WebAPIKurs.Shared.Entities;

namespace WebAPIKurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnTypesController : ControllerBase
    {
        private readonly MovieDbContext _context;

        public ReturnTypesController(MovieDbContext context)
        {
            _context = context;
        }

        #region String-Ausgaben

        //Synchron
        [HttpGet("GetHelloWord")]
        public string GetHelloWord()
        {
            return "Hello World";
        }

        [HttpGet("GetHelloWorld2")]
        public ContentResult GetHelloWorld2()
        {
            return Content("Hello World");
        }

        #endregion
        #region Complexen Typen


        //WebAPI kann Objekt automatisch als JSON ausgeben.
        //Allerdings können wir keine definierten Fehlermeldungen herausgeben. 
        //JSON 
        [HttpGet("GetComplexObject")]
        public Car GetComplexObject()
        {
            Car car = new()
            {
                Id = 1,
                Brand = "Porsche",
                Model = "911"
            };

            return car;
        }
        #endregion
        #region Synchrone Methode

        //IActionResult - ActionResult


        [HttpGet("GetCarWithIActionResult")]
        public IActionResult GetCarWithIActionResult()
        {
            Car car = new()
            {
                Id = 1,
                Brand = "Porsche",
                Model = "911"
            };

            if (car == null)
                return NotFound("Car wurde nicht gefunden"); //404

            if (car.Brand == "BMW")
            {
                return BadRequest("Eine Fehlermeldung kann man hier eingeben");
            }


            //OK = StatusCode: 200
            return Ok(car);
        }

        [HttpGet("GetCarWithActionResult")]
        public ActionResult GetCarWithActionResult()
        {
            Car car = new()
            {
                Id = 1,
                Brand = "Porsche",
                Model = "911"
            };

            if (car == null)
                return NotFound("Car wurde nicht gefunden"); //404

            if (car.Brand == "BMW")
            {
                return BadRequest("Eine Fehlermeldung kann man hier eingeben");
            }


            //OK = StatusCode: 200
            return Ok(car);
        }


        [HttpGet("GetCarWithGenericActionResult")]
        public ActionResult<Car> GetCarWithGenericActionResult()
        {
            Car car = new()
            {
                Id = 1,
                Brand = "Porsche",
                Model = "911"
            };

            if (car == null)
                return NotFound(); //404

            return Ok(car); //200 
        }

        #endregion
        #region Asynchrone Methoden

        [HttpGet("GetCarWithIActionResultAsync")]
        public async Task<IActionResult> GetCarWithIActionResultAsync()
        {

            await Task.Delay(1000);
            Car car = new()
            {
                Id = 1,
                Brand = "Porsche",
                Model = "911"
            };

            if (car == null)
                return NotFound();


            return Ok(car);
        }

        #endregion

        #region IEnumerable-List

        [HttpGet("GetAllMoviesSync")]
        public IEnumerable<Movie> GetMoviesWithIEnumerable()
        {
            var movies = _context.Movie.ToList();

            foreach (var movie in movies)
            {
                yield return movie;
            }
        }
        #endregion

        #region IEnumerableAsync
        //Geht nur in Verbindung mit EFCore-Package

        [HttpGet("GetAllMoviesAsync")]
        public async IAsyncEnumerable<Movie> GetMoviesWithIEnumerableAsync()
        {
            var movies = _context.Movie.AsAsyncEnumerable();

            await foreach(var movie in movies)
            {
                yield return movie;
            }
        }
        #endregion


        #region IList


        [HttpGet("GetMovieList")]
        public async Task<IList<Movie>> GetMovieList()
        {
            return await _context.Movie.ToListAsync();
        }

        #endregion
    }
}

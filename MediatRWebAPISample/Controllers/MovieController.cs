using MediatR;
using MediatRWebAPISample.Commands;
using MediatRWebAPISample.Models;
using MediatRWebAPISample.Notifications;
using MediatRWebAPISample.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediatRWebAPISample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovieController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]   
        public async Task<ActionResult> GetMovies()
        {
            // 
            IEnumerable<Movie> movies = await _mediator.Send(new GetMoviesQuery());
            return Ok(movies);
        }

        [HttpGet("{id:int}", Name = "GetMovieById")]
        public async Task<ActionResult> GetMovieById(int id)
        {
            Movie movie = await _mediator.Send(new GetMovieByIdQuery(id));

            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult> AddMovie(Movie movie)
        {
            //Workflow1 -> Hinzufügen eines Films
            Movie movieWithId = await _mediator.Send(new AddMovieCommand(movie));


            //Weitere Workflows mit INotification -> kann an mehrere Handler rauskommen 
            await _mediator.Publish(new MovieAddedNotification(movieWithId));


            return CreatedAtRoute("GetMovieById", new { id = movieWithId.Id }, movieWithId);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WebAPIKurs.Shared.Entities;

namespace WebAPIKurs.MVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }


        public async Task<IActionResult> Index()
        {
            List<Movie> movies = await _movieService.GetMoviesAsync();

            return View(movies);
        }

        public async Task<IActionResult> Details(int id)
        {
            Movie? movie = await _movieService.GetMovieAsync(id);
            if (movie is null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            await _movieService.CreateMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Movie? movie = await _movieService.GetMovieAsync(id);
            if (movie is null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            await _movieService.UpdateMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Movie? movie = await _movieService.GetMovieAsync(id);
            if (movie is null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movieService.DeleteMovieAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

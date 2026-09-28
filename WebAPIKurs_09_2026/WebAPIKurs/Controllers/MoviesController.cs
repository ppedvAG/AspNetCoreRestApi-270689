using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Cryptography;
using WebAPIKurs.Models;
using WebAPIKurs.Shared.Entities;


/// <summary>
/// Movie-Controller Klasse (CRUD)
/// </summary>
/// 

[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Verwaltung aller Kunden")]
public class MoviesController : ControllerBase
{
    private readonly MovieDbContext _context;
    public MoviesController(MovieDbContext context)
    {
        _context = context;
    }

    // GET: api/Movie
    [HttpGet]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
    {
        return await _context.Movie.ToListAsync();
    }

    
    /// <summary>
    /// GetMovie mit Id ist wirklich sehr schön
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _context.Movie.FindAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        return movie;
    }

    // PUT: api/Movie/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public async Task<IActionResult> PutMovie(int? id, Movie movie)
    {

        //movie.id (ist auf dem HTTP-Body) HTTP-Body beinhaltet den gesamten Datensatz 
        if (id != movie.Id)
        {
            return BadRequest();
        }

        _context.Entry(movie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Movie
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    
    
    [HttpPost]  
    //Data Annotations bei Model-Binding
    public async Task<ActionResult<Movie>> PostMovie(Movie movie)
    {

        if (movie.Price < 0)
            ModelState.AddModelError("Price", "Kein negativer Preis"); //ModelState.IsValid 

        
        //MUSS
        if (!ModelState.IsValid)
        {
            return BadRequest("Ups da ist was schief gelaufen");
        }



        _context.Movie.Add(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
    }

    // DELETE: api/Movie/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int? id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Movie.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MovieExists(int? id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }
}

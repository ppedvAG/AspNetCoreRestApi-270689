using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ODataWebAPISamples.Data;
using ODataWebAPISamples.Models;

namespace ODataWebAPISamples.Controllers
{

    public class MoviesController (MovieDbContext db): ODataController
    {
        //odata/Movies?$filter=Genre eq MovieApp.Shared.Entities.GenreType'Action'
        /// <summary>
        /// IQueryable ist wichtig: OData kann die Abfrageoptionen auf die
        /// Datenquelle anwenden, bevor die Ergebnisse gelesen werden.
        /// </summary>
        [EnableQuery(PageSize = 100)]
        [HttpGet]
        public IQueryable<Movie> GetMovies()
        {
            return db.Movies;
        }
    }
}

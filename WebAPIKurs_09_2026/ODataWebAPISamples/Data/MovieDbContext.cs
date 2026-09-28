using Microsoft.EntityFrameworkCore;
using ODataWebAPISamples.Models;

namespace ODataWebAPISamples.Data
{
    /// <summary>
    /// EF-Core-Datenbank für das OData-Beispiel.
    /// InMemory bedeutet: Die Daten liegen nur im Arbeitsspeicher und werden beim
    /// nächsten Start erneut aus den Seed-Daten erzeugt.
    /// </summary>
    public class MovieDbContext(DbContextOptions<MovieDbContext> options) : DbContext(options)
    {
        public DbSet<Movie> Movies => Set<Movie>();
    }
}

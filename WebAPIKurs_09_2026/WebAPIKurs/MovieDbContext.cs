using Microsoft.EntityFrameworkCore;
using WebAPIKurs.Shared.Entities;

public class MovieDbContext(DbContextOptions<MovieDbContext> options) : DbContext(options)
{
    public DbSet<Movie> Movie { get; set; } = default!;
}

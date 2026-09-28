
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ODataWebAPISamples.Data;
using ODataWebAPISamples.Models;

namespace ODataWebAPISamples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            IEdmModel edmModel = CreateEdmModel();
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseInMemoryDatabase("ODataMovies"));
            builder.Services.AddControllers()
                .AddOData(options => options
                    .Select()
                    .Filter()
                    .OrderBy()
                    .Count()
                    .SetMaxTop(100)
                    .AddRouteComponents("harry", edmModel));
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Die Beispieldaten werden bei jedem Start in die flüchtige Datenbank gelegt.
            using (IServiceScope scope = app.Services.CreateScope())
            {
                MovieDbContext db = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
                db.Database.EnsureCreated();

                if (!db.Movies.Any())
                {
                    db.Movies.AddRange(
                        new Movie { Title = "The Matrix", Description = "Eine Hackerin entdeckt eine simulierte Welt.", Price = 9.99m, Genre = GenreType.Action },
                        new Movie { Title = "The Mask", Description = "Eine magische Maske verändert das Leben ihres Trägers.", Price = 7.50m, Genre = GenreType.Comedy },
                        new Movie { Title = "Toy Story", Description = "Spielzeuge erleben ein großes Abenteuer.", Price = 8.99m, Genre = GenreType.Animation },
                        new Movie { Title = "Planet Earth", Description = "Dokumentation über faszinierende Lebensräume.", Price = 11.99m, Genre = GenreType.Documentary },
                        new Movie { Title = "The Shining", Description = "Ein abgelegenes Hotel birgt ein düsteres Geheimnis.", Price = 6.99m, Genre = GenreType.Horror },
                        new Movie { Title = "Interstellar", Description = "Eine Reise durch Raum und Zeit soll die Menschheit retten.", Price = 10.99m, Genre = GenreType.ScienceFiction });
                    db.SaveChanges();
                }
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        static IEdmModel CreateEdmModel()
        {
            ODataConventionModelBuilder modelBuilder = new();
            modelBuilder.EntitySet<Movie>("Movies");
            return modelBuilder.GetEdmModel();
        }
    }
}


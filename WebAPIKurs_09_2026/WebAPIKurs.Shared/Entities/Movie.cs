using System.ComponentModel.DataAnnotations;
using WebAPIKurs.Shared.Attributes;

namespace WebAPIKurs.Shared.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Der Titel fehlt")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Der Client fehlt")]
        public string Description { get; set; }
        public decimal Price { get; set; }


        [ClassicMovie(1960)]
        public int? Year { get; set; }
        public GenreType Genre { get; set;  }
    }

    public enum GenreType { Action, Comedy, Drame, Family, Classics }
}

using System.ComponentModel.DataAnnotations;
using WebAPIKurs.Shared.Entities;


namespace WebAPIKurs.Shared.Attributes
{
    public class ClassicMovieAttribute : ValidationAttribute
    {
        //Kriterium
        public int Year { get; set;  }

        public ClassicMovieAttribute(int year)
        {
            Year = year;
        }

        private string GetErrorMessage() => $"Filmklassiker sind alle vor {Year} erschienen";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            Movie movie = (Movie)validationContext.ObjectInstance;

            //Welches Jahr wurde eingegeben
            int releaseYear = (int)value;

            if (movie.Genre == GenreType.Classics && releaseYear > Year)
                return new ValidationResult(GetErrorMessage());

            return ValidationResult.Success;
        }
    }
}

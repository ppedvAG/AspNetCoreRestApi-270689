namespace ODataWebAPISamples.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public GenreType Genre { get; set; }
    }

    public enum GenreType { Action, Drama, Comedy, Animation, Documentary, Thriller, Horror, ScienceFiction, Fantasy }

}

namespace WebAPIKurs.Shared.Entities
{
    public interface IMovieService
    {
        Task<List<Movie>> GetMoviesAsync();
        Task<List<Movie>> GetMoviesXmalAsync();

        Task<Movie?> GetMovieAsync(int id);
        Task CreateMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);

        Task DeleteMovieAsync(int id);
    }
}

using MediatRWebAPISample.Models;

namespace MediatRWebAPISample.Data
{
    public class FakeDataStore
    {
        private static List<Movie> _movies;

        public FakeDataStore()
        {
            _movies = new List<Movie>();
            _movies.Add(new Movie() { Id = 1, Title = "Jurassic World", Description = "Dinosauer", Price = 9.99m, Genre = GenreType.Action });
            _movies.Add(new Movie() { Id = 2, Title = "Star Wars", Description = "Imperium ist böse", Price = 19.99m, Genre = GenreType.ScienceFiction });
            _movies.Add(new Movie() { Id = 3, Title = "Top Gun Maverick", Description = "Ist besser als Teil 1", Price = 15.99m, Genre = GenreType.Action });
        }


        #region Backend Data-Access Queries
        public async Task<IEnumerable<Movie>> GetAllMovies() => await Task.FromResult(_movies);

        public async Task<Movie> GetMovieById(int id) => await Task.FromResult(_movies.Single(p => p.Id == id));

        public async Task AddMovie(Movie movie)
        {
            _movies.Add(movie);
            await Task.CompletedTask;
        }
        #endregion


        public async Task EventOccured(Movie movie, string evt)
        {
            _movies.Single(p => p.Id == movie.Id).Title = $"{movie.Title} evt: {evt}";
            await Task.CompletedTask;
        }
    }
}

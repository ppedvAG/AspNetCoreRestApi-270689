using System.Xml.Serialization;
using WebAPIKurs.Shared.Entities;

namespace WebAPIKurs.MVC.Services
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;

        public MovieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            using HttpRequestMessage request = new(HttpMethod.Get, "api/movies");
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<Movie>>() ?? [];
        }

        public async Task<Movie?> GetMovieAsync(int id)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, $"api/movies/{id}");
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Movie>();
        }

        public async Task<List<Movie>> GetMoviesXmalAsync()
        {
            //1.) Erstelle das HttpRequestMessage
            HttpRequestMessage request = new(HttpMethod.Get, "api/movies");
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));

            //2.)Versenden und erhalten Respnse
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string xml = await response.Content.ReadAsStringAsync();

            XmlSerializer serializer = new(typeof(List<Movie>));

            using StringReader reader = new StringReader(xml);


            //DAs Ausrufezeichen ist ein Null-Forgiving Operator (Null-Vergebungsoperator).
            return (List<Movie>)serializer.Deserialize(reader)!;
        }



        public async Task CreateMovieAsync(Movie movie)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync("api/movies", movie);
            result.EnsureSuccessStatusCode();

        }

        public async Task DeleteMovieAsync(int id)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/movies/{id}");
            result.EnsureSuccessStatusCode();
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"api/movies/{movie.Id}", movie);
            result.EnsureSuccessStatusCode();
        }
    }
}

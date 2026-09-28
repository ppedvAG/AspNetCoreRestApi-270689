using MediatR;
using MediatRWebAPISample.Data;
using MediatRWebAPISample.Models;
using MediatRWebAPISample.Queries;

namespace MediatRWebAPISample.Handlers
{
    public class GetMoviesHandler : IRequestHandler<GetMoviesQuery, IEnumerable<Movie>>
    {

        private readonly FakeDataStore _fakeDataStore;

        public GetMoviesHandler(FakeDataStore fakeDataStore)
        {
            _fakeDataStore = fakeDataStore;
        }


        public async Task<IEnumerable<Movie>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            //FakeDB
            //Gebe alle Daten zurück 
            return await _fakeDataStore.GetAllMovies();
        }
    }
}

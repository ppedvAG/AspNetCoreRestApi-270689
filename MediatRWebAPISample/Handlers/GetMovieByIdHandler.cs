using MediatR;
using MediatRWebAPISample.Data;
using MediatRWebAPISample.Models;
using MediatRWebAPISample.Queries;

namespace MediatRWebAPISample.Handlers
{
    public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, Movie>
    {
        private readonly FakeDataStore _fakeDataStore;

        public GetMovieByIdHandler(FakeDataStore fakeDataStore) => _fakeDataStore = fakeDataStore;


        public async Task<Movie> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            Movie movie = await _fakeDataStore.GetMovieById(request.Id);

            return movie;
        }
    }
}

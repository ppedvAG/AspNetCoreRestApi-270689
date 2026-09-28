using MediatR;
using MediatRWebAPISample.Commands;
using MediatRWebAPISample.Data;
using MediatRWebAPISample.Models;

namespace MediatRWebAPISample.Handlers
{
    public class AddMovieHandler : IRequestHandler<AddMovieCommand, Movie>
    {
        private readonly FakeDataStore _fakeDataStore;
        private readonly IMediator _mediator;
        
        public AddMovieHandler(FakeDataStore fakeDataStore, IMediator mediator)
        {
            _fakeDataStore = fakeDataStore;
            _mediator = mediator;   
        }
        

        public async Task<Movie> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            await _fakeDataStore.AddMovie(request.Movie); //Referenz wird verwendet und ID wird vom SERVER vergeben und man kann Movie mit ID verwenden. 

            return request.Movie;
        }
    }
}

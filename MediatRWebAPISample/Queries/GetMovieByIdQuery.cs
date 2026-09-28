using MediatR;
using MediatRWebAPISample.Models;

namespace MediatRWebAPISample.Queries
{
    //Wir übergeben eine Id und Erhalt
    public record GetMovieByIdQuery(int Id) : IRequest<Movie>;
}

using MediatR;
using MediatRWebAPISample.Models;

namespace MediatRWebAPISample.Commands
{
    //Parameter ist Movie und das Request ist das Movie (mit einer ID) 
    public record AddMovieCommand(Movie Movie) : IRequest<Movie>;
}

using MediatR;
using MediatRWebAPISample.Models;

namespace MediatRWebAPISample.Queries
{
    //Rückgabe (Ergebnis meiner Abfrage) ist: IEnumerable<Movie>
    public class GetMoviesQuery : IRequest<IEnumerable<Movie>>
    {

    }
}

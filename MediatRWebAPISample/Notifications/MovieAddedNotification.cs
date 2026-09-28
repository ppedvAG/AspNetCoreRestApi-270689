using MediatR;
using MediatRWebAPISample.Models;

namespace MediatRWebAPISample.Notifications
{
    public record MovieAddedNotification (Movie Movie) : INotification;
    
}

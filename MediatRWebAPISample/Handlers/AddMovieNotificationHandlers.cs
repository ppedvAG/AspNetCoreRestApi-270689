using MediatR;
using MediatRWebAPISample.Data;
using MediatRWebAPISample.Notifications;

namespace MediatRWebAPISample.Handlers
{
    public class EmailHandler : INotificationHandler<MovieAddedNotification>
    {
        private readonly FakeDataStore _fakeDataStore;
        //private readonly IMediator _mediator;

        public EmailHandler(FakeDataStore fakeDataStore)
        {
            _fakeDataStore = fakeDataStore;
        }

        public async Task Handle(MovieAddedNotification notification, CancellationToken cancellationToken)
        {
            await _fakeDataStore.EventOccured(notification.Movie, "Email sent");

            await Task.CompletedTask;  
        }
    }

    public class CacheInvalidationHandler : INotificationHandler<MovieAddedNotification>
    {
        private readonly FakeDataStore _fakeDataStore;

        public CacheInvalidationHandler(FakeDataStore fakeDataStore) => _fakeDataStore = fakeDataStore;

        public async Task Handle(MovieAddedNotification notification, CancellationToken cancellationToken)
        {
            await _fakeDataStore.EventOccured(notification.Movie, "Cache Invalidated");

            await Task.CompletedTask;


        }
    }
}

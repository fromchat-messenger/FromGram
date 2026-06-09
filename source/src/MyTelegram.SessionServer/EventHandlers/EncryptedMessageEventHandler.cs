using MyTelegram.Abstractions;
using MyTelegram.EventBus;
using MyTelegram.SessionServer.Services;

namespace MyTelegram.SessionServer.EventHandlers;

public sealed class EncryptedMessageEventHandler(ISessionDataProcessor dataProcessor)
    : IEventHandler<EncryptedMessage>, ITransientDependency
{
    public async Task HandleEventAsync(EncryptedMessage eventData)
    {
        await dataProcessor.EnqueueAsync(eventData).ConfigureAwait(false);
    }
}

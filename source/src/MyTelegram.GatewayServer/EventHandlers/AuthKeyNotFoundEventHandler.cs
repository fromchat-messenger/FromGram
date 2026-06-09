namespace MyTelegram.GatewayServer.EventHandlers;

public class AuthKeyNotFoundEventHandler(IClientDataSender clientDataSender, IClientManager clientManager)
    : IEventHandler<AuthKeyNotFoundEvent>, ITransientDependency
{
    // 0x6c, 0xfe, 0xff, 0xff
    private static readonly byte[] AuthKeyNotFoundData = [0x6c, 0xfe, 0xff, 0xff]; //-404

    public async Task HandleEventAsync(AuthKeyNotFoundEvent eventData)
    {
        // During DH handshake the connection has no auth key yet; -404 would abort the handshake loop.
        if (eventData.AuthKeyId == 0)
        {
            return;
        }

        if (!clientManager.TryGetClientData(eventData.ConnectionId, out _))
        {
            return;
        }

        var m = new EncryptedMessageResponse(eventData.AuthKeyId, AuthKeyNotFoundData, eventData.ConnectionId, 2);
        await clientDataSender.SendAsync(m);
    }
}
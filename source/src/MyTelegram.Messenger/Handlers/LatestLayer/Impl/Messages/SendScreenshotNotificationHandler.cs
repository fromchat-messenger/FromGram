namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Messages;

///<summary>
/// Notify the other user in a private chat that a screenshot of the chat was taken.
/// <para>Possible errors</para>
/// Code Type Description
/// 400 PEER_ID_INVALID The provided peer id is invalid.
/// 400 YOU_BLOCKED_USER You blocked this user.
/// See <a href="https://corefork.telegram.org/method/messages.sendScreenshotNotification" />
///</summary>
internal sealed class SendScreenshotNotificationHandler(
    IAccessHashHelper accessHashHelper)
    : RpcResultObjectHandler<MyTelegram.Schema.Messages.RequestSendScreenshotNotification, MyTelegram.Schema.IUpdates>,
        Messages.ISendScreenshotNotificationHandler
{
    protected override async Task<IUpdates> HandleCoreAsync(IRequestInput input,
        RequestSendScreenshotNotification obj)
    {
        await accessHashHelper.CheckAccessHashAsync(input, obj.Peer);
        // Screenshot notification is a service message action.
        // Currently returns empty updates as the notification is acknowledged.
        return new TUpdates
        {
            Updates = [],
            Users = [],
            Chats = [],
            Date = CurrentDate,
            Seq = 0
        };
    }
}

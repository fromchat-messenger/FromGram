namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Messages;

///<summary>
/// Delete scheduled messages.
/// <para>Possible errors</para>
/// Code Type Description
/// 400 PEER_ID_INVALID The provided peer id is invalid.
/// See <a href="https://corefork.telegram.org/method/messages.deleteScheduledMessages" />
///</summary>
internal sealed class DeleteScheduledMessagesHandler(
    IAccessHashHelper accessHashHelper)
    : RpcResultObjectHandler<MyTelegram.Schema.Messages.RequestDeleteScheduledMessages, MyTelegram.Schema.IUpdates>,
        Messages.IDeleteScheduledMessagesHandler
{
    protected override async Task<IUpdates> HandleCoreAsync(IRequestInput input,
        RequestDeleteScheduledMessages obj)
    {
        await accessHashHelper.CheckAccessHashAsync(input, obj.Peer);
        // Scheduled messages deletion requires scheduled message infrastructure
        // which is not yet available in the domain
        throw new RpcException(new RpcError(400, "SCHEDULE_DATE_INVALID"));
    }
}

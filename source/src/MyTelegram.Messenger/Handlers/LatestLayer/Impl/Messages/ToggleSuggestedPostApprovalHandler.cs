namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Messages;

///<summary>
/// See <a href="https://corefork.telegram.org/method/messages.toggleSuggestedPostApproval" />
///</summary>
internal sealed class ToggleSuggestedPostApprovalHandler
    : RpcResultObjectHandler<MyTelegram.Schema.Messages.RequestToggleSuggestedPostApproval, MyTelegram.Schema.IUpdates>,
        MyTelegram.Messenger.Handlers.Messages.IToggleSuggestedPostApprovalHandler
{
    protected override Task<MyTelegram.Schema.IUpdates> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Messages.RequestToggleSuggestedPostApproval obj)
    {
        // Suggested post approval is not yet supported
        throw new RpcException(new RpcError(400, "PEER_ID_INVALID"));
    }
}

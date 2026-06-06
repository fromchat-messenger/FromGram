namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Messages;

///<summary>
/// See <a href="https://corefork.telegram.org/method/messages.toggleTodoCompleted" />
///</summary>
internal sealed class ToggleTodoCompletedHandler
    : RpcResultObjectHandler<MyTelegram.Schema.Messages.RequestToggleTodoCompleted, MyTelegram.Schema.IUpdates>,
        MyTelegram.Messenger.Handlers.Messages.IToggleTodoCompletedHandler
{
    protected override Task<MyTelegram.Schema.IUpdates> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Messages.RequestToggleTodoCompleted obj)
    {
        // Todo list completion toggle is not yet fully supported
        throw new RpcException(new RpcError(400, "PEER_ID_INVALID"));
    }
}

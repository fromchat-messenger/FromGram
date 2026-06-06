namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl;

///<summary>
/// See <a href="https://corefork.telegram.org/method/invokeWithMessagesRange" />
///</summary>
internal sealed class InvokeWithMessagesRangeHandler
    : RpcResultObjectHandler<MyTelegram.Schema.RequestInvokeWithMessagesRange, IObject>,
        IInvokeWithMessagesRangeHandler
{
    protected override Task<IObject> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.RequestInvokeWithMessagesRange obj)
    {
        throw new RpcException(new RpcError(400, "INPUT_METHOD_INVALID"));
    }
}

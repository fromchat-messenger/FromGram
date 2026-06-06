namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl;

///<summary>
/// Invokes a query after a successful completion of previous queries.
/// See <a href="https://corefork.telegram.org/method/invokeAfterMsgs" />
///</summary>
internal sealed class InvokeAfterMsgsHandler
    : RpcResultObjectHandler<MyTelegram.Schema.RequestInvokeAfterMsgs, IObject>,
        IInvokeAfterMsgsHandler
{
    protected override Task<IObject> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.RequestInvokeAfterMsgs obj)
    {
        // invokeAfterMsgs is a wrapper that ensures query ordering.
        // The actual query inside is executed by the transport layer.
        // This handler should not normally be reached.
        throw new RpcException(new RpcError(400, "INPUT_METHOD_INVALID"));
    }
}

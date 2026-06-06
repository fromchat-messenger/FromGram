namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl;

///<summary>
/// See <a href="https://corefork.telegram.org/method/invokeWithBusinessConnection" />
///</summary>
internal sealed class InvokeWithBusinessConnectionHandler
    : RpcResultObjectHandler<MyTelegram.Schema.RequestInvokeWithBusinessConnection, IObject>,
        IInvokeWithBusinessConnectionHandler
{
    protected override Task<IObject> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.RequestInvokeWithBusinessConnection obj)
    {
        throw new RpcException(new RpcError(400, "INPUT_METHOD_INVALID"));
    }
}

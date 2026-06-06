namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl;

///<summary>
/// See <a href="https://corefork.telegram.org/method/invokeWithApnsSecret" />
///</summary>
internal sealed class InvokeWithApnsSecretHandler
    : RpcResultObjectHandler<MyTelegram.Schema.RequestInvokeWithApnsSecret, IObject>,
        IInvokeWithApnsSecretHandler
{
    protected override Task<IObject> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.RequestInvokeWithApnsSecret obj)
    {
        throw new RpcException(new RpcError(400, "INPUT_METHOD_INVALID"));
    }
}

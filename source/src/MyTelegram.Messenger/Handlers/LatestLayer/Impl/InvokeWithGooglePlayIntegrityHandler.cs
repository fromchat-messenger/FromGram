namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl;

///<summary>
/// See <a href="https://corefork.telegram.org/method/invokeWithGooglePlayIntegrity" />
///</summary>
internal sealed class InvokeWithGooglePlayIntegrityHandler
    : RpcResultObjectHandler<MyTelegram.Schema.RequestInvokeWithGooglePlayIntegrity, IObject>,
        IInvokeWithGooglePlayIntegrityHandler
{
    protected override Task<IObject> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.RequestInvokeWithGooglePlayIntegrity obj)
    {
        throw new RpcException(new RpcError(400, "INPUT_METHOD_INVALID"));
    }
}

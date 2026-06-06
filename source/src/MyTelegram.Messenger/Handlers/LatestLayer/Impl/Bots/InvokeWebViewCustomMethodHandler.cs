namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Bots;

///<summary>
/// Invoke a custom method in a web view.
/// See <a href="https://corefork.telegram.org/method/bots.invokeWebViewCustomMethod" />
///</summary>
internal sealed class InvokeWebViewCustomMethodHandler
    : RpcResultObjectHandler<MyTelegram.Schema.Bots.RequestInvokeWebViewCustomMethod, MyTelegram.Schema.IDataJSON>,
        Bots.IInvokeWebViewCustomMethodHandler
{
    protected override Task<IDataJSON> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Bots.RequestInvokeWebViewCustomMethod obj)
    {
        throw new RpcException(new RpcError(400, "BOT_INVALID"));
    }
}

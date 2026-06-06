namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl;

///<summary>
/// See <a href="https://corefork.telegram.org/method/invokeWithReCaptcha" />
///</summary>
internal sealed class InvokeWithReCaptchaHandler
    : RpcResultObjectHandler<MyTelegram.Schema.RequestInvokeWithReCaptcha, IObject>,
        IInvokeWithReCaptchaHandler
{
    protected override Task<IObject> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.RequestInvokeWithReCaptcha obj)
    {
        throw new RpcException(new RpcError(400, "INPUT_METHOD_INVALID"));
    }
}

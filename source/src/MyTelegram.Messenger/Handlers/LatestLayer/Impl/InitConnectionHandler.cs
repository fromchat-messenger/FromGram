namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl;

///<summary>
/// Initialize connection and save information on the user\'s device and application.
/// See <a href="https://corefork.telegram.org/method/initConnection" />
///</summary>
internal sealed class InitConnectionHandler
    : RpcResultObjectHandler<MyTelegram.Schema.RequestInitConnection, IObject>,
        IInitConnectionHandler
{
    protected override Task<IObject> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.RequestInitConnection obj)
    {
        // initConnection is a wrapper method handled by the transport layer.
        // This handler should not normally be reached directly.
        throw new RpcException(new RpcError(400, "INPUT_METHOD_INVALID"));
    }
}

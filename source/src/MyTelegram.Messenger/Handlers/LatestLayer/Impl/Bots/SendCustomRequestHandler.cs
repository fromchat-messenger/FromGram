namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Bots;

///<summary>
/// Sends a custom request from a bot.
/// See <a href="https://corefork.telegram.org/method/bots.sendCustomRequest" />
///</summary>
internal sealed class SendCustomRequestHandler
    : RpcResultObjectHandler<MyTelegram.Schema.Bots.RequestSendCustomRequest, MyTelegram.Schema.IDataJSON>,
        Bots.ISendCustomRequestHandler
{
    protected override Task<IDataJSON> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Bots.RequestSendCustomRequest obj)
    {
        // Custom bot requests require webhook infrastructure
        throw new RpcException(new RpcError(400, "BOT_INVALID"));
    }
}

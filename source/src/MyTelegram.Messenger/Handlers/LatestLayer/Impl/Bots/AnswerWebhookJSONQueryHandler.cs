namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Bots;

///<summary>
/// Answer an inline query sent from a web app.
/// See <a href="https://corefork.telegram.org/method/bots.answerWebhookJSONQuery" />
///</summary>
internal sealed class AnswerWebhookJSONQueryHandler
    : RpcResultObjectHandler<MyTelegram.Schema.Bots.RequestAnswerWebhookJSONQuery, IBool>,
        Bots.IAnswerWebhookJSONQueryHandler
{
    protected override Task<IBool> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Bots.RequestAnswerWebhookJSONQuery obj)
    {
        throw new RpcException(new RpcError(400, "QUERY_ID_INVALID"));
    }
}

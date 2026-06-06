namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Bots;

///<summary>
/// Update the emoji status of a user.
/// See <a href="https://corefork.telegram.org/method/bots.updateUserEmojiStatus" />
///</summary>
internal sealed class UpdateUserEmojiStatusHandler
    : RpcResultObjectHandler<MyTelegram.Schema.Bots.RequestUpdateUserEmojiStatus, IBool>,
        Bots.IUpdateUserEmojiStatusHandler
{
    protected override Task<IBool> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Bots.RequestUpdateUserEmojiStatus obj)
    {
        // Emoji status update requires bot with appropriate permissions
        throw new RpcException(new RpcError(400, "BOT_INVALID"));
    }
}

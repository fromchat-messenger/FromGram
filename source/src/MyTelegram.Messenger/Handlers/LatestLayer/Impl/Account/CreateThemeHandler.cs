namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Account;

///<summary>
/// Create a theme.
/// See <a href="https://corefork.telegram.org/method/account.createTheme" />
///</summary>
internal sealed class CreateThemeHandler
    : RpcResultObjectHandler<MyTelegram.Schema.Account.RequestCreateTheme, MyTelegram.Schema.ITheme>,
        Account.ICreateThemeHandler
{
    protected override Task<ITheme> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Account.RequestCreateTheme obj)
    {
        // Theme creation requires file storage infrastructure
        throw new RpcException(new RpcError(400, "THEME_INVALID"));
    }
}

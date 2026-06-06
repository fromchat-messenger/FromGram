namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Account;

///<summary>
/// Update theme.
/// See <a href="https://corefork.telegram.org/method/account.updateTheme" />
///</summary>
internal sealed class UpdateThemeHandler
    : RpcResultObjectHandler<MyTelegram.Schema.Account.RequestUpdateTheme, MyTelegram.Schema.ITheme>,
        Account.IUpdateThemeHandler
{
    protected override Task<ITheme> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Account.RequestUpdateTheme obj)
    {
        throw new RpcException(new RpcError(400, "THEME_INVALID"));
    }
}

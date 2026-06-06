namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Account;

///<summary>
/// Edit a business chat link.
/// See <a href="https://corefork.telegram.org/method/account.editBusinessChatLink" />
///</summary>
internal sealed class EditBusinessChatLinkHandler
    : RpcResultObjectHandler<MyTelegram.Schema.Account.RequestEditBusinessChatLink, MyTelegram.Schema.IBusinessChatLink>,
        Account.IEditBusinessChatLinkHandler
{
    protected override Task<MyTelegram.Schema.IBusinessChatLink> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Account.RequestEditBusinessChatLink obj)
    {
        throw new RpcException(new RpcError(400, "SLUG_INVALID"));
    }
}

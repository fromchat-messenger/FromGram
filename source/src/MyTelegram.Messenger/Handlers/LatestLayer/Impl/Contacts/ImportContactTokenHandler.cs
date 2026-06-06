namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Contacts;

///<summary>
/// Import a contact token, to add a user to our contact list.
/// <para>Possible errors</para>
/// Code Type Description
/// 400 IMPORT_TOKEN_INVALID The specified token is invalid.
/// See <a href="https://corefork.telegram.org/method/contacts.importContactToken" />
///</summary>
internal sealed class ImportContactTokenHandler(
    IUserConverterService userConverterService)
    : RpcResultObjectHandler<MyTelegram.Schema.Contacts.RequestImportContactToken, MyTelegram.Schema.IUser>,
        Contacts.IImportContactTokenHandler
{
    protected override Task<IUser> HandleCoreAsync(IRequestInput input,
        RequestImportContactToken obj)
    {
        // Contact tokens are a feature for importing contacts by token.
        // Not yet implemented - token validation requires token generation infrastructure.
        throw new RpcException(new RpcError(400, "IMPORT_TOKEN_INVALID"));
    }
}

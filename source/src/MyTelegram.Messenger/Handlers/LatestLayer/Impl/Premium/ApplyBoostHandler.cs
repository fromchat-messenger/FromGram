namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Premium;

///<summary>
/// Apply a boost to a peer.
/// See <a href="https://corefork.telegram.org/method/premium.applyBoost" />
///</summary>
internal sealed class ApplyBoostHandler
    : RpcResultObjectHandler<MyTelegram.Schema.Premium.RequestApplyBoost, MyTelegram.Schema.Premium.IMyBoosts>,
        Premium.IApplyBoostHandler
{
    protected override Task<MyTelegram.Schema.Premium.IMyBoosts> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Premium.RequestApplyBoost obj)
    {
        // Boosting requires Premium infrastructure
        throw new RpcException(new RpcError(400, "PREMIUM_ACCOUNT_REQUIRED"));
    }
}

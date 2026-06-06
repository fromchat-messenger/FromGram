namespace MyTelegram.Messenger.Handlers.LatestLayer.Impl.Channels;

///<summary>
/// Edit forum topic; requires manage_topics rights.
/// <para>Possible errors</para>
/// Code Type Description
/// 400 TOPIC_ID_INVALID The specified topic ID is invalid.
/// 400 TOPIC_NOT_MODIFIED The updated topic info is equal to the current topic info.
/// See <a href="https://corefork.telegram.org/method/channels.editForumTopic" />
///</summary>
internal sealed class EditForumTopicHandler(
    IAccessHashHelper accessHashHelper,
    IChannelAdminRightsChecker channelAdminRightsChecker)
    : RpcResultObjectHandler<MyTelegram.Schema.Channels.RequestEditForumTopic, MyTelegram.Schema.IUpdates>,
        Channels.IEditForumTopicHandler
{
    protected override async Task<IUpdates> HandleCoreAsync(IRequestInput input,
        MyTelegram.Schema.Channels.RequestEditForumTopic obj)
    {
        if (obj.Channel is TInputChannel inputChannel)
        {
            await accessHashHelper.CheckAccessHashAsync(input, inputChannel.ChannelId, inputChannel.AccessHash, AccessHashType.Channel);
            await channelAdminRightsChecker.CheckAdminRightAsync(inputChannel.ChannelId, input.UserId,
                p => p.AdminRights.ManageTopics, RpcErrors.RpcErrors403.ChatAdminRequired);

            // Forum topic editing requires forum domain infrastructure
            throw new RpcException(new RpcError(400, "TOPIC_ID_INVALID"));
        }

        throw new RpcException(RpcErrors.RpcErrors400.ChannelInvalid);
    }
}

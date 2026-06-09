using Microsoft.Extensions.Options;
using MyTelegram.Core;
using MyTelegram.EventBus;
using MyTelegram.Schema;
using MyTelegram.Schema.Extensions;
using MyTelegram.SessionServer.Options;

namespace MyTelegram.SessionServer.Services.Impl;

/// <summary>
/// Reconstructed from the original binary's SessionDataDispatcher.
/// Routes deserialized RPC requests to the correct downstream server
/// by publishing typed DataReceivedEvent subtypes via the event bus.
///
/// Routing logic (from ObjectIdConsts):
///   - ObjectId in CommandServerHandlers → MessengerCommandDataReceivedEvent
///   - ObjectId in StickerServerObjectIds → StickerDataReceivedEvent
///   - Otherwise → MessengerQueryDataReceivedEvent (default)
/// </summary>
public sealed class SessionDataDispatcher : ISessionDataDispatcher
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<SessionDataDispatcher> _logger;
    private readonly IOptionsMonitor<MyTelegramSessionServerOptions> _options;

    public SessionDataDispatcher(
        IEventBus eventBus,
        ILogger<SessionDataDispatcher> logger,
        IOptionsMonitor<MyTelegramSessionServerOptions> options)
    {
        _eventBus = eventBus;
        _logger = logger;
        _options = options;
    }

    public async Task DispatchAsync(InternalSessionData sessionData)
    {
        var objectId = sessionData.ObjectId;
        var input = sessionData.RequestInput;
        var data = sessionData.RequestData.ToBytes() ?? [];

        _logger.LogDebug(
            "Dispatching objectId=0x{ObjectId:X8} user={UserId} authKey={AuthKeyId} dataLen={DataLen}",
            objectId, input.UserId, input.AuthKeyId, data.Length);

        if (ObjectIdConsts.CommandServerHandlers.ContainsKey(objectId))
        {
            await _eventBus.PublishAsync(CreateCommandEvent(input, objectId, data))
                .ConfigureAwait(false);
        }
        else if (IsStickerObjectId(objectId))
        {
            await _eventBus.PublishAsync(CreateStickerEvent(input, objectId, data))
                .ConfigureAwait(false);
        }
        else
        {
            await _eventBus.PublishAsync(CreateQueryEvent(input, objectId, data))
                .ConfigureAwait(false);
        }
    }

    private bool IsStickerObjectId(uint objectId)
    {
        var opts = _options.CurrentValue;
        return opts.StickerServerObjectIds.Contains(objectId);
    }

    private static MessengerQueryDataReceivedEvent CreateQueryEvent(
        IRequestInput input, uint objectId, byte[] data) =>
        new(input.ConnectionId, input.RequestId, objectId, input.UserId, input.ReqMsgId,
            input.SeqNumber, input.AuthKeyId, input.PermAuthKeyId, data, input.Layer,
            input.Date, input.DeviceType, input.ClientIp, input.SessionId, input.AccessHashKeyId);

    private static MessengerCommandDataReceivedEvent CreateCommandEvent(
        IRequestInput input, uint objectId, byte[] data) =>
        new(input.ConnectionId, input.RequestId, objectId, input.UserId, input.ReqMsgId,
            input.SeqNumber, input.AuthKeyId, input.PermAuthKeyId, data, input.Layer,
            input.Date, input.DeviceType, input.ClientIp, input.SessionId, input.AccessHashKeyId);

    private static StickerDataReceivedEvent CreateStickerEvent(
        IRequestInput input, uint objectId, byte[] data) =>
        new(input.ConnectionId, input.RequestId, objectId, input.UserId, input.ReqMsgId,
            input.SeqNumber, input.AuthKeyId, input.PermAuthKeyId, data, input.Layer,
            input.Date, input.DeviceType, input.ClientIp, input.SessionId, input.AccessHashKeyId);
}

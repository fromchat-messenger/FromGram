namespace MyTelegram.SessionServer.Services;

/// <summary>
/// Generates MTProto server-side message_id values.
/// MTProto message_id = (unixtime_in_seconds * 2^32) | (seqno * 4)
/// Message IDs must be even (bit 0 = 0) for server-originated messages.
/// </summary>
public static class MessageIdGenerator
{
    private static long _counter;

    public static long GenerateServerMessageId()
    {
        var unixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var seq = Interlocked.Increment(ref _counter) & 0x7FFFFF; // 23-bit counter
        // Server responses to client queries use msg_id where (msg_id % 4) == 1.
        var messageId = (unixSeconds << 32) | ((seq << 2) & 0xFFFFFFFC);
        if ((messageId & 3) != 1)
            messageId += 1;
        return messageId;
    }

    /// <summary>
    /// Validates a client-supplied message_id.
    /// Client message_ids must be divisible by 4 (see MTProto message identifier rules).
    /// Additionally, msg_id should be reasonably close to server time.
    /// </summary>
    public static bool IsValidClientMessageId(long messageId)
    {
        if ((messageId & 3) != 0)
            return false;

        // Check that the timestamp portion is within ±300 seconds of now
        var msgTime = messageId >> 32;
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var diff = Math.Abs(now - msgTime);
        return diff <= 300;
    }
}

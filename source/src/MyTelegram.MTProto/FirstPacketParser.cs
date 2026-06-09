namespace MyTelegram.MTProto;

public class FirstPacketParser(
    ILogger<FirstPacketParser> logger,
    IAesHelper aesHelper)
    : IFirstPacketParser, ITransientDependency
{
    private const byte AbridgedFlag = 0xef;
    private const byte IntermediateFlag = 0xee;
    private const int ConnectionPrefixBytes = 64;
    private static readonly byte[] AbridgedBytes = [AbridgedFlag, AbridgedFlag, AbridgedFlag, AbridgedFlag];

    private static readonly byte[] IntermediateBytes =
        [IntermediateFlag, IntermediateFlag, IntermediateFlag, IntermediateFlag];

    public FirstPacketData Parse(ReadOnlySpan<byte> firstPacket)
    {
        logger.LogInformation("DEBUG: [1] Received {Len} bytes: {Hex}", firstPacket.Length, Convert.ToHexString(firstPacket));

        if (firstPacket.Length < ConnectionPrefixBytes)
        {
            return ParseUnObfuscationFirstPacket(firstPacket);
        }

        var nonce = firstPacket[..ConnectionPrefixBytes];
        var sendKey = firstPacket.Slice(8, 32).ToArray();
        var sendIv = firstPacket.Slice(40, 16).ToArray();
        
        logger.LogInformation("DEBUG: [2] SendKey: {K}", Convert.ToHexString(sendKey));
        logger.LogInformation("DEBUG: [3] SendIv: {IV}", Convert.ToHexString(sendIv));

        // Правильный реверс для receiveKey/IV (согласно спецификации MTProto)
        Span<byte> reversedBytes = stackalloc byte[48];
        for (int i = 0; i < 48; i++)
        {
            reversedBytes[i] = firstPacket[8 + (47 - i)];
        }
        
        logger.LogInformation("DEBUG: [4] ReversedBytes: {RB}", Convert.ToHexString(reversedBytes));

        var receiveKey = reversedBytes[..32].ToArray();
        var receiveIv = reversedBytes.Slice(32, 16).ToArray();

        logger.LogInformation("DEBUG: [5] Final ReceiveKey: {K}", Convert.ToHexString(receiveKey));
        logger.LogInformation("DEBUG: [6] Final ReceiveIv: {IV}", Convert.ToHexString(receiveIv));

        var encryptedNonce = ArrayPool<byte>.Shared.Rent(nonce.Length);
        try
        {
            logger.LogInformation("DEBUG: [7] Starting AES-CTR decryption");
            aesHelper.CtrEncrypt(nonce, encryptedNonce, sendKey, sendIv, 0);

            logger.LogInformation("DEBUG: [8] Decrypted Nonce (first 64 bytes): {Hex}", Convert.ToHexString(encryptedNonce.AsSpan(0, 64)));

            var data = new FirstPacketData
            {
                ObfuscationEnabled = true,
                ProtocolBufferLength = ConnectionPrefixBytes,
                // Client advances only the send (encrypt) CTR stream by 64 while building the init packet.
                // decryptNum stays at 0 until the first server response is received.
                SendCount = ConnectionPrefixBytes,
                ReceiveCount = 0
            };
            var protocolBytes = encryptedNonce.AsSpan().Slice(56, 4);

            if (protocolBytes.SequenceEqual([AbridgedFlag, AbridgedFlag, AbridgedFlag, AbridgedFlag]))
                data.ProtocolType = ProtocolType.Abridge;
            else if (protocolBytes.SequenceEqual([IntermediateFlag, IntermediateFlag, IntermediateFlag, IntermediateFlag]))
                data.ProtocolType = ProtocolType.Intermediate;

            if (data.ProtocolType != ProtocolType.Unknown)
            {
                if (encryptedNonce[56] == 0xef && encryptedNonce[57] == 0xef && 
                    encryptedNonce[58] == 0xef && encryptedNonce[59] == 0xef) 
                {
                    // Это маркер конца обфускации. 
                    // Настоящий dcId будет прислан СЕРВЕРОМ в ответе на ReqPq, 
                    // либо он уже определен на стороне клиента.
                    logger.LogInformation("Obfuscation handshake complete. DC ID not in this packet.");
                }
                
                data.SendKey = sendKey;
                data.SendIv = sendIv;
                data.ReceiveKey = receiveKey;
                data.ReceiveIv = receiveIv;
            }
            return data;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(encryptedNonce);
        }
    }

    private FirstPacketData ParseUnObfuscationFirstPacket(ReadOnlySpan<byte> firstPacket)
    {
        var state = new FirstPacketData
        {
            ObfuscationEnabled = false,
            ProtocolBufferLength = 1
        };
        byte protocolByte = firstPacket[0];
        ProtocolType protocolType = ProtocolType.Unknown;
        switch (protocolByte)
        {
            case AbridgedFlag:
                protocolType = ProtocolType.Abridge;
                break;
            case IntermediateFlag:
                protocolType = ProtocolType.Intermediate;
                break;
            default:
                logger.LogWarning("UnKnown protocol: {Protocol}", firstPacket[0]);
                break;
        }

        if (firstPacket.Length == 4)
        {
            state.ProtocolBufferLength = 4;
        }

        state.ProtocolType = protocolType;

        logger.LogInformation("[{ProtocolType}](UnObfuscation) detected", state.ProtocolType);

        return state;
    }
}

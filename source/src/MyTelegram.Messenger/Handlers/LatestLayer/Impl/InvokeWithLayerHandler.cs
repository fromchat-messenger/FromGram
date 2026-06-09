// ReSharper disable All

namespace MyTelegram.Handlers;

using Microsoft.Extensions.Logging;

///<summary>
/// Invoke the specified query using the specified API <a href="https://corefork.telegram.org/api/invoking#layers">layer</a>
/// <para>Possible errors</para>
/// Code Type Description
/// 400 AUTH_BYTES_INVALID The provided authorization is invalid.
/// 400 CDN_METHOD_INVALID You can't call this method in a CDN DC.
/// 403 CHAT_WRITE_FORBIDDEN You can't write in this chat.
/// 400 CONNECTION_API_ID_INVALID The provided API id is invalid.
/// 406 INVITE_HASH_EXPIRED The invite link has expired.
/// See <a href="https://corefork.telegram.org/method/invokeWithLayer" />
///</summary>

internal sealed class InvokeWithLayerHandler(
    IHandlerHelper handlerHelper,
    IEventBus eventBus,
    ILogger<InvokeWithLayerHandler> logger
    )
    : BaseObjectHandler<RequestInvokeWithLayer, IObject>,
        IInvokeWithLayerHandler
{
    private void LogToRoot(string message)
    {
        // Запись в файл прямо в рабочую директорию контейнера
        string path = "debug_invoke.log";
        try 
        {
            // Используем true для дозаписи в файл (Append)
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CRITICAL] Ошибка записи в файл: {ex.Message}");
        }

        Console.WriteLine(message);
    }

    protected override async Task<IObject> HandleCoreAsync(IRequestInput input, RequestInvokeWithLayer obj)
    {
        // 1. Сначала проверяем сам контейнер и его содержимое
        if (obj == null)
        {
            LogToRoot("InvokeWithLayer: obj is null!");
            throw new ArgumentNullException(nameof(obj));
        }

        // 1. Логируем сам факт прихода InvokeWithLayer
        // Используем Convert.ToString(obj.Query.ConstructorId, 16) для красоты
        LogToRoot($"[InvokeWithLayer] Received. Layer: {obj.Layer}, Query ID: 0x{obj.Query?.ConstructorId:x8}");

        if (obj.Query == null)
        {
            LogToRoot("InvokeWithLayer: Query is NULL!");
            throw new ArgumentNullException(nameof(obj.Query));
        }

        IObject query = obj.Query;

        // 2. Логируем, если это InitConnection
        if (obj.Query is RequestInitConnection initConnection)
        {
            LogToRoot($"[InvokeWithLayer] Detected InitConnection (ApiId: {initConnection.ApiId}, AppVersion: {initConnection.AppVersion})");
            query = initConnection.Query;
            await SaveDeviceInfoAsync(input, initConnection.ApiId, initConnection.AppVersion, initConnection.DeviceModel, initConnection.SystemVersion, initConnection.SystemLangCode, initConnection.LangPack, initConnection.LangCode);
        }
        else if (obj.Query is Schema.LayerN.RequestInitConnection initConnectionLayerN)
        {
            LogToRoot($"[InvokeWithLayer] Detected InitConnection (LayerN) (ApiId: {initConnectionLayerN.ApiId})");
            query = initConnectionLayerN.Query;
            await SaveDeviceInfoAsync(input, initConnectionLayerN.ApiId, initConnectionLayerN.AppVersion, initConnectionLayerN.DeviceModel, initConnectionLayerN.SystemVersion, initConnectionLayerN.SystemLangCode, initConnectionLayerN.LangPack, initConnectionLayerN.LangCode);
        }

        if (query == null)
        {
            LogToRoot("InvokeWithLayer: Inner query is null after extraction!");
            throw new ArgumentException("InitConnection.query can not be null.");
        }

        // 3. Логируем перед поиском хендлера
        LogToRoot($"[InvokeWithLayer] Attempting to find handler for inner query ID: 0x{query.ConstructorId:x8}");

        if (!handlerHelper.TryGetHandler(query.ConstructorId, out var handler))
        {
            LogToRoot($"[InvokeWithLayer] No handler found for query ID: 0x{query.ConstructorId:x8}");
            throw new NotSupportedException($"Not supported query: 0x{query.ConstructorId:x8}");
        }

        handlerHelper.TryGetHandlerShortName(query.ConstructorId, out var handlerShortName);
        LogToRoot($"[InvokeWithLayer] Found handler: {handlerShortName ?? "Unknown"} for ID: 0x{query.ConstructorId:x8}. Executing...");

        var result = await handler.HandleAsync(input, query);
        LogToRoot($"[InvokeWithLayer] Handler executed successfully.");

        return result;
    }

    private async Task SaveDeviceInfoAsync(IRequestInput requestInput,
        int apiId,
        string appVersion,
        string deviceModel,
        string systemVersion,
        string systemLangCode,
        string langPack,
        string langCode
    )
    {
        if (requestInput.PermAuthKeyId == 0)
        {
            return;
        }

        

        var eventData = new NewDeviceCreatedEvent(
            requestInput.ToRequestInfo(), 
            requestInput.PermAuthKeyId, 
            requestInput.AuthKeyId,
            requestInput.UserId,
            apiId,
            "FromGram", // AppName - добавлено
            appVersion,
            0,          // Hash
            false,      // OfficialApp
            false,      // PasswordPending
            deviceModel,
            "Linux",    // Platform - добавлено (твой сервер на Linux)
            systemVersion,
            systemLangCode,
            langPack,
            langCode,
            requestInput.ClientIp,
            requestInput.Layer,
            null        // Parameters
        );
        await eventBus.PublishAsync(eventData);
    }
}
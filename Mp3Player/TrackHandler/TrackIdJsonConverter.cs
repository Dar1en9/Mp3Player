using System.Text.Json; 
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Mp3Player.TrackHandler;

public class TrackIdJsonConverter(ILogger logger) : JsonConverter<TrackId>
{
    public override TrackId Read(ref Utf8JsonReader reader, Type typeToConvert, 
        JsonSerializerOptions options) 
    {
        logger.LogDebug("Начало десериализации TrackId");
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            logger.LogError("Ожидался StartObject, но получен {TokenType}", reader.TokenType);
            throw new JsonException();
        }
        reader.Read(); // Read StartObject
        reader.Read(); // Read PropertyName (id)
        var guid = reader.GetGuid(); 
        logger.LogDebug("Получен GUID: {Guid}", guid);
        reader.Read(); // Read EndObject
        var trackId = new TrackId(guid);
        logger.LogDebug("Создан TrackId: {TrackId}", trackId);
        return trackId;
    }

    public override void Write(Utf8JsonWriter writer, TrackId value, JsonSerializerOptions options)
    {
        logger.LogDebug("Начало сериализации TrackId: {TrackId}", value);
        writer.WriteStartObject(); 
        writer.WriteString("Id", value.Id); 
        writer.WriteEndObject();
        logger.LogDebug("Завершение сериализации TrackId: {TrackId}", value);
    }
}

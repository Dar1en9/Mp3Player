using System.Text.Json; 
using System.Text.Json.Serialization;

namespace Mp3Player.TrackHandler;

public class TrackIdJsonConverter : JsonConverter<TrackId>
{
    public override TrackId Read(ref Utf8JsonReader reader, Type typeToConvert, 
        JsonSerializerOptions options) 
    {
        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();
        reader.Read(); // Read StartObject
        reader.Read(); // Read PropertyName (id)
        var guid = reader.GetGuid(); 
        reader.Read(); // Read EndObject
        return new TrackId(guid);
    }

    public override void Write(Utf8JsonWriter writer, TrackId value, JsonSerializerOptions options)
    {
        writer.WriteStartObject(); 
        writer.WriteString("Id", value.Id); 
        writer.WriteEndObject();
    }
}

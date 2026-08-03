using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackendDotnet.DTOs;

public sealed class UtcDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var parsedValue = reader.GetDateTime();
        return parsedValue.Kind == DateTimeKind.Utc
            ? parsedValue
            : DateTime.SpecifyKind(parsedValue, DateTimeKind.Utc);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var utcValue = value.Kind == DateTimeKind.Utc
            ? value
            : value.ToUniversalTime();

        writer.WriteStringValue(utcValue.ToString("O", CultureInfo.InvariantCulture));
    }
}
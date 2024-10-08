using System.Text.Json;
using System.Text.Json.Serialization;
using ContractChecker;
using ContractChecker.JsonConverters;

public class JsonOptionalConverter : JsonConverter<object>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return (typeToConvert.IsClass || (typeToConvert.IsValueType && !typeToConvert.IsPrimitive))
        && typeToConvert.FindInterfaces((_, _) => true, null).Contains(typeof(IContract));
    }

    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        var type = value.GetType();
        var properties = type.GetProperties();

        foreach (var prop in properties)
        {
            if (prop.IsDefined(typeof(JsonOptionalAttribute), true))
            {
                continue;
            }

            var propValue = prop.GetValue(value);
            writer.WritePropertyName(prop.Name);
            JsonSerializer.Serialize(writer, propValue, prop.PropertyType, options);
        }

        writer.WriteEndObject();
    }
}

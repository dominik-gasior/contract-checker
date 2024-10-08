using System.Text.Json;

namespace ContractChecker;

internal class JsonExtensions
{
    public static string Serialize(object obj)
        => JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonOptionalConverter() }
        });

    public static bool CompareJson(string json1, string json2)
    {
        using (JsonDocument doc1 = JsonDocument.Parse(json1))
        using (JsonDocument doc2 = JsonDocument.Parse(json2))
        {
            return JsonElementEquals(doc1.RootElement, doc2.RootElement);
        }
    }

    private static bool JsonElementEquals(JsonElement element1, JsonElement element2)
    {
        if (element1.ValueKind != element2.ValueKind)
            return false;

        switch (element1.ValueKind)
        {
            case JsonValueKind.Object:
                {
                    var properties1 = element1.EnumerateObject();
                    var properties2 = element2.EnumerateObject();

                    var dict1 = new Dictionary<string, JsonElement>();
                    var dict2 = new Dictionary<string, JsonElement>();

                    foreach (var prop in properties1)
                        dict1[prop.Name] = prop.Value;

                    foreach (var prop in properties2)
                        dict2[prop.Name] = prop.Value;

                    if (dict1.Count != dict2.Count)
                        return false;

                    foreach (var kvp in dict1)
                    {
                        if (!dict2.TryGetValue(kvp.Key, out var value2))
                            return false;

                        if (!JsonElementEquals(kvp.Value, value2))
                            return false;
                    }

                    return true;
                }

            case JsonValueKind.Array:
                {
                    var array1 = element1.EnumerateArray();
                    var array2 = element2.EnumerateArray();

                    var list1 = new List<JsonElement>(array1);
                    var list2 = new List<JsonElement>(array2);

                    if (list1.Count != list2.Count)
                        return false;

                    for (int i = 0; i < list1.Count; i++)
                    {
                        if (!JsonElementEquals(list1[i], list2[i]))
                            return false;
                    }

                    return true;
                }

            case JsonValueKind.String:
                return element1.GetString() == element2.GetString();

            case JsonValueKind.Number:
                return element1.GetDecimal() == element2.GetDecimal();

            case JsonValueKind.True:
            case JsonValueKind.False:
                return element1.GetBoolean() == element2.GetBoolean();

            case JsonValueKind.Null:
                return true;

            default:
                return false;
        }
    }
}

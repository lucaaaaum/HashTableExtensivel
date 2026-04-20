using System.Text.Json;

namespace HashTableExtensivel.Json;

public static class Serializador
{
    private static readonly JsonSerializerOptions opções = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNameCaseInsensitive = true
    };

    public static string Serializar<T>(T valor) => JsonSerializer.Serialize(valor, opções);

    public static T? Desserializar<T>(string json) => JsonSerializer.Deserialize<T>(json, opções);
}

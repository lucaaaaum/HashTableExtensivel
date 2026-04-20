namespace HashTableExtensivel.Json;

public class WrapperGenérico<T>(T? valor)
{
    public string? Tipo = typeof(T).FullName;
    public T? Valor { get; set; } = valor;
}

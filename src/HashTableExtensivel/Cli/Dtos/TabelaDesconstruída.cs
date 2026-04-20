using HashTableExtensivel.EstruturasDeDados;

namespace HashTableExtensivel.Cli.Dtos;

public class TabelaDesconstruída
{
    public int TamanhoDoBucket { get; set; }
    public required string[] Chaves { get; set; }
    public required string[] Elementos { get; set; }

    public static TabelaDesconstruída Converter(HashTable<string, string> tabela)
    {
        var itens = tabela
            .ObterDiretório()
            .SelectMany(bucket => bucket.ObterChavesEElementos().Where(item => item is not null))
            .ToList();
        return new TabelaDesconstruída
        {
            TamanhoDoBucket = tabela.TamanhoDoBucket,
            Chaves = [.. itens.Select(item => item!.Value.chave?.ToString() ?? string.Empty)],
            Elementos = [.. itens.Select(item => item!.Value.elemento?.ToString() ?? string.Empty)]
        };
    }
}

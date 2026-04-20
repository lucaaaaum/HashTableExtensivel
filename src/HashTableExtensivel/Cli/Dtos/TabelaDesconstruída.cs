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
            .Distinct()
            .ToList();
        return new TabelaDesconstruída
        {
            TamanhoDoBucket = tabela.TamanhoDoBucket,
            Chaves = [.. itens.Select(item => item!.Value.chave?.ToString() ?? string.Empty)],
            Elementos = [.. itens.Select(item => item!.Value.elemento?.ToString() ?? string.Empty)]
        };
    }

    public HashTable<string, string> Converter()
    {
        var tabela = new HashTable<string, string>(TamanhoDoBucket);
        for (int i = 0; i < Chaves.Length; i++)
            tabela.Inserir(Chaves[i], Elementos[i]);
        return tabela;
    }
}

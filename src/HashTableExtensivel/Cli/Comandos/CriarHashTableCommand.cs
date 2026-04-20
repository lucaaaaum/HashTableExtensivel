using HashTableExtensivel.Cli.Dtos;
using HashTableExtensivel.EstruturasDeDados;
using HashTableExtensivel.Json;
using Spectre.Console.Cli;

namespace HashTableExtensivel.Cli.Comandos;

public class CriarHashTableSettings : CommandSettings
{
    [CommandOption("-t|--tamanho")]
    public int TamanhoDoBucket { get; init; } = 2;

    [CommandOption("-a|--arquivo")]
    public string? Arquivo { get; init; }
}

public class CriarHashTableCommand : Command<CriarHashTableSettings>
{
    protected override int Execute(CommandContext context, CriarHashTableSettings settings, CancellationToken cancellationToken)
    {
        if (File.Exists(settings.Arquivo))
        {
            Console.WriteLine($"O arquivo '{settings.Arquivo}' já existe. Por favor, escolha um nome de arquivo diferente ou remova o arquivo existente.");
            return 1;
        }

        if (settings.TamanhoDoBucket <= 0)
        {
            Console.WriteLine("O tamanho do bucket deve ser um número inteiro positivo.");
            return 1;
        }

        var arquivo = Path.ChangeExtension(settings.Arquivo, "json") ?? $"hash_table.json";

        var hashTableString = new HashTable<string, string>(settings.TamanhoDoBucket);
        SalvarHashTableEmArquivo(hashTableString, arquivo);

        return 0;
    }

    private static void SalvarHashTableEmArquivo(HashTable<string, string> hashTable, string arquivo)
    {
        var tabelaDesconstruída = TabelaDesconstruída.Converter(hashTable);
        var json = Serializador.Serializar(tabelaDesconstruída);
        File.WriteAllText(arquivo, json);
    }
}

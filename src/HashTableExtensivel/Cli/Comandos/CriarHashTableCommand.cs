using System.Text.Encodings.Web;
using System.Text.Json;
using HashTableExtensivel.Cli.Dtos;
using HashTableExtensivel.EstruturasDeDados;
using Spectre.Console.Cli;

namespace HashTableExtensivel.Cli.Comandos;

public class CriarHashTableSettings : CommandSettings
{
    [CommandOption("-t|--tamanho")]
    public int TamanhoDoBucket { get; init; } = 2;

    [CommandOption("-a|--arquivo")]
    public string? Arquivo { get; init; }

    [CommandOption("-c|--chave")]
    public TipoDeDado Chave { get; init; } = TipoDeDado.Int;

    [CommandOption("-e|--elemento")]
    public TipoDeDado Elemento { get; init; } = TipoDeDado.String;
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

        if (!Enum.IsDefined(settings.Chave))
        {
            Console.WriteLine("Tipo de chave inválido. Por favor, escolha um tipo de chave válido.");
            return 1;
        }

        if (!Enum.IsDefined(settings.Elemento))
        {
            Console.WriteLine("Tipo de elemento inválido. Por favor, escolha um tipo de elemento válido.");
            return 1;
        }

        var arquivo = Path.ChangeExtension(settings.Arquivo, "json") ?? $"hash_table_{settings.Chave.ToString().ToLower()}_{settings.TamanhoDoBucket}.json";

        switch (settings.Chave)
        {
            case TipoDeDado.Int:
                var hashTableInt = new HashTable<int, string>(settings.TamanhoDoBucket);
                SalvarHashTableEmArquivo(settings.Chave, settings.Elemento, hashTableInt, arquivo);
                break;
            case TipoDeDado.String:
                var hashTableString = new HashTable<string, string>(settings.TamanhoDoBucket);
                SalvarHashTableEmArquivo(settings.Chave, settings.Elemento, hashTableString, arquivo);
                break;
        }

        return 0;
    }

    private void SalvarHashTableEmArquivo<TChave, TElemento>(TipoDeDado tipoDeChave, TipoDeDado tipoDeElemento, HashTable<TChave, TElemento> hashTable, string arquivo)
    {
        var tabelaComMetadados = new TabelaComMetadados
        {
            Tabela = hashTable,
            TipoDeChave = tipoDeChave,
            TipoDeElemento = tipoDeElemento
        };
        var json = JsonSerializer.Serialize(tabelaComMetadados, new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        File.WriteAllText(arquivo, json);
    }
}

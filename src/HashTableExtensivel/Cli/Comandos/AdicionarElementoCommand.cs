using System.Text.Encodings.Web;
using System.Text.Json;
using HashTableExtensivel.Cli.Dtos;
using HashTableExtensivel.EstruturasDeDados;
using Spectre.Console.Cli;

namespace HashTableExtensivel.Cli.Comandos;

public class AdicionarElementoSettings : CommandSettings
{
    [CommandOption("-a|--arquivo", isRequired: true)]
    public string? Arquivo { get; init; }

    [CommandOption("-c|--chave", isRequired: true)]
    public string? Chave { get; init; }

    [CommandOption("-e|--elemento", isRequired: true)]
    public string? Elemento { get; init; }
}

public class AdicionarElementoCommand : Command<AdicionarElementoSettings>
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    protected override int Execute(CommandContext context, AdicionarElementoSettings settings, CancellationToken cancellationToken)
    {
        var arquivo = Path.ChangeExtension(settings.Arquivo, "json") ?? settings.Arquivo;

        if (!File.Exists(arquivo))
        {
            Console.WriteLine($"O arquivo '{arquivo}' não existe. Por favor, verifique o nome do arquivo e tente novamente.");
            return 1;
        }

        var tabelaComMetadados = JsonSerializer.Deserialize<TabelaComMetadados>(File.ReadAllText(arquivo));

        if (tabelaComMetadados is null)
        {
            Console.WriteLine($"O arquivo '{arquivo}' não contém uma tabela válida. Por favor, verifique o conteúdo do arquivo e tente novamente.");
            return 1;
        }

        var chaveInt = 0;
        if (tabelaComMetadados.TipoDeChave is TipoDeDado.Int && !int.TryParse(settings.Chave, out chaveInt))
        {
            Console.WriteLine($"A chave '{settings.Chave}' não é um número inteiro válido. Por favor, verifique a chave e tente novamente.");
            return 1;
        }

        var elementoInt = 0;
        if (tabelaComMetadados.TipoDeElemento is TipoDeDado.Int && !int.TryParse(settings.Elemento, out elementoInt))
        {
            Console.WriteLine($"O elemento '{settings.Elemento}' não é um número inteiro válido. Por favor, verifique o elemento e tente novamente.");
            return 1;
        }

        if (tabelaComMetadados.TipoDeChave is TipoDeDado.String && string.IsNullOrWhiteSpace(settings.Chave))
        {
            Console.WriteLine($"A chave não pode ser nula. Por favor, verifique a chave e tente novamente.");
            return 1;
        }

        if (tabelaComMetadados.TipoDeElemento is TipoDeDado.String && string.IsNullOrWhiteSpace(settings.Elemento))
        {
            Console.WriteLine($"O elemento não pode ser nulo. Por favor, verifique o elemento e tente novamente.");
            return 1;
        }

        switch ((tabelaComMetadados.TipoDeChave, tabelaComMetadados.TipoDeElemento))
        {
            case (TipoDeDado.Int, TipoDeDado.Int):
                var hashTableIntInt = JsonSerializer.Deserialize<HashTable<int, int>>(JsonSerializer.Serialize(tabelaComMetadados.Tabela, jsonSerializerOptions), jsonSerializerOptions);
                hashTableIntInt!.Inserir(chaveInt, elementoInt);
                break;
            case (TipoDeDado.Int, TipoDeDado.String):
                var hashTableIntString = JsonSerializer.Deserialize<HashTable<int, string>>(JsonSerializer.Serialize(tabelaComMetadados.Tabela, jsonSerializerOptions), jsonSerializerOptions);
                hashTableIntString!.Inserir(chaveInt, settings.Elemento!);
                break;
            case (TipoDeDado.String, TipoDeDado.Int):
                var hashTableStringInt = JsonSerializer.Deserialize<HashTable<string, int>>(JsonSerializer.Serialize(tabelaComMetadados.Tabela, jsonSerializerOptions), jsonSerializerOptions);
                hashTableStringInt!.Inserir(settings.Chave!, elementoInt);
                break;
            case (TipoDeDado.String, TipoDeDado.String):
                var hashTableStringString = JsonSerializer.Deserialize<HashTable<string, string>>(JsonSerializer.Serialize(tabelaComMetadados.Tabela, jsonSerializerOptions), jsonSerializerOptions);
                hashTableStringString!.Inserir(settings.Chave!, settings.Elemento!);
                break;
        }

        var tabelaComMetadadosAtualizada = new TabelaComMetadados
        {
            Tabela = tabelaComMetadados.Tabela,
            TipoDeChave = tabelaComMetadados.TipoDeChave,
            TipoDeElemento = tabelaComMetadados.TipoDeElemento
        };
        var json = JsonSerializer.Serialize(tabelaComMetadadosAtualizada, jsonSerializerOptions);

        File.WriteAllText(arquivo, json);

        return 0;
    }
}

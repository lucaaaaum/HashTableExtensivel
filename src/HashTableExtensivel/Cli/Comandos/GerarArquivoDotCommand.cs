using HashTableExtensivel.Cli.Dtos;
using HashTableExtensivel.Graphviz;
using HashTableExtensivel.Json;
using Spectre.Console.Cli;

namespace HashTableExtensivel.Cli.Comandos;

public class GerarArquivoDotCommandSettings : CommandSettings
{
    [CommandOption("-a|--arquivo", isRequired: true)]
    public string? Arquivo { get; init; }

    [CommandOption("-o|--output")]
    public string? Output { get; init; }
}

public class GerarArquivoDotCommand : Command<GerarArquivoDotCommandSettings>
{
    protected override int Execute(CommandContext context, GerarArquivoDotCommandSettings settings, CancellationToken cancellationToken)
    {
        if (!File.Exists(settings.Arquivo))
        {
            Console.WriteLine($"O arquivo '{settings.Arquivo}' não existe. Por favor, verifique o nome do arquivo e tente novamente.");
            return 1;
        }

        var outputFileName = string.IsNullOrWhiteSpace(settings.Output) ? Path.ChangeExtension(settings.Arquivo, "dot") : settings.Output;

        var tabelaDesconstruída = Serializador.Desserializar<TabelaDesconstruída>(File.ReadAllText(settings.Arquivo));
        var tabela = tabelaDesconstruída!.Converter();

        var dotContent = GeradorDeArquivoDot.GerarArquivoDot(tabela);

        File.WriteAllText(outputFileName, dotContent);

        return 0;
    }
}

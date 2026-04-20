using HashTableExtensivel.Graphviz;
using Spectre.Console.Cli;

namespace HashTableExtensivel.Cli.Comandos;

public class GerarSvgCommandSettings : CommandSettings
{
    [CommandOption("-a|--arquivo", isRequired: true)]
    public string? Arquivo { get; init; }

    [CommandOption("-o|--output")]
    public string? Output { get; init; }
}

public class GerarSvgCommand : Command<GerarSvgCommandSettings>
{
    protected override int Execute(CommandContext context, GerarSvgCommandSettings settings, CancellationToken cancellationToken)
    {
        if (!File.Exists(settings.Arquivo))
        {
            Console.WriteLine($"O arquivo '{settings.Arquivo}' não existe. Por favor, verifique o nome do arquivo e tente novamente.");
            return 1;
        }

        var caminhoOutput = string.IsNullOrWhiteSpace(settings.Output) ? Path.ChangeExtension(settings.Arquivo, "svg") : settings.Output;

        var caminhoDot = Path.ChangeExtension(settings.Arquivo, "dot");
        if (!File.Exists(caminhoDot))
        {
            Console.WriteLine($"O arquivo DOT '{caminhoDot}' não existe. Por favor, gere o arquivo DOT usando o comando 'gerar-dot' antes de gerar o SVG.");
            return 1;
        }

        Dot.GerarArquivoSvg(caminhoDot, caminhoOutput);

        return 0;
    }
}

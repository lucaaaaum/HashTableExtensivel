using HashTableExtensivel.Cli.Dtos;
using HashTableExtensivel.Json;
using Spectre.Console.Cli;

namespace HashTableExtensivel.Cli.Comandos;

public class BuscarCommandSettings : CommandSettings
{
    [CommandOption("-a|--arquivo", isRequired: true)]
    public string? Arquivo { get; init; }

    [CommandOption("-c|--chave", isRequired: true)]
    public string? Chave { get; init; }
}

public class BuscarCommand : Command<BuscarCommandSettings>
{
    protected override int Execute(CommandContext context, BuscarCommandSettings settings, CancellationToken cancellationToken)
    {
        if (!File.Exists(settings.Arquivo))
        {
            Console.WriteLine($"O arquivo '{settings.Arquivo}' não existe. Por favor, verifique o nome do arquivo e tente novamente.");
            return 1;
        }

        if (string.IsNullOrWhiteSpace(settings.Chave))
        {
            Console.WriteLine("A chave não pode ser vazia. Por favor, forneça uma chave válida e tente novamente.");
            return 1;
        }

        var tabelaDesconstruída = Serializador.Desserializar<TabelaDesconstruída>(File.ReadAllText(settings.Arquivo));
        var tabela = tabelaDesconstruída!.Converter();

        var elemento = tabela.Buscar(settings.Chave);

        if (elemento is null)
        {
            Console.WriteLine($"A chave '{settings.Chave}' não foi encontrada na tabela.");
            return 0;
        }

        Console.WriteLine($"Chave: {settings.Chave}, Elemento: {elemento}");

        return 0;
    }
}

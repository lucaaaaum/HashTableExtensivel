using HashTableExtensivel.Cli.Dtos;
using HashTableExtensivel.Json;
using Spectre.Console.Cli;

namespace HashTableExtensivel.Cli.Comandos;

public class RemoverElementoCommandSettings : CommandSettings
{
    [CommandOption("-a|--arquivo", isRequired: true)]
    public string? Arquivo { get; init; }

    [CommandOption("-c|--chave", isRequired: true)]
    public string? Chave { get; init; }
}

public class RemoverElementoCommand : Command<RemoverElementoCommandSettings>
{
    protected override int Execute(CommandContext context, RemoverElementoCommandSettings settings, CancellationToken cancellationToken)
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

        tabela.Remover(settings.Chave);

        var tabelaDesconstruídaAtualizada = TabelaDesconstruída.Converter(tabela);
        var json = Serializador.Serializar(tabelaDesconstruídaAtualizada);

        File.WriteAllText(settings.Arquivo, json);

        return 0;
    }
}

using HashTableExtensivel.Cli.Dtos;
using HashTableExtensivel.EstruturasDeDados;
using HashTableExtensivel.Json;
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
    protected override int Execute(CommandContext context, AdicionarElementoSettings settings, CancellationToken cancellationToken)
    {
        var arquivo = Path.ChangeExtension(settings.Arquivo, "json") ?? settings.Arquivo;

        if (!File.Exists(arquivo))
        {
            Console.WriteLine($"O arquivo '{arquivo}' não existe. Por favor, verifique o nome do arquivo e tente novamente.");
            return 1;
        }

        if (string.IsNullOrWhiteSpace(settings.Chave))
        {
            Console.WriteLine("A chave não pode ser vazia. Por favor, forneça uma chave válida e tente novamente.");
            return 1;
        }

        if (string.IsNullOrWhiteSpace(settings.Elemento))
        {
            Console.WriteLine("O elemento não pode ser vazio. Por favor, forneça um elemento válido e tente novamente.");
            return 1;
        }

        var tabelaDesconstruída = Serializador.Desserializar<TabelaDesconstruída>(File.ReadAllText(arquivo));
        var tabela = new HashTable<string, string>(tabelaDesconstruída!.TamanhoDoBucket);
        foreach (var item in tabelaDesconstruída.Chaves.Zip(tabelaDesconstruída.Elementos, (chave, elemento) => (chave, elemento)))
            tabela.Inserir(item.chave, item.elemento);

        if (tabela is null)
        {
            Console.WriteLine($"O arquivo '{arquivo}' não contém uma tabela válida. Por favor, verifique o conteúdo do arquivo e tente novamente.");
            return 1;
        }

        tabela.Inserir(settings.Chave, settings.Elemento);

        var tabelaDesconstruídaAtualizada = TabelaDesconstruída.Converter(tabela);
        var json = Serializador.Serializar(tabelaDesconstruídaAtualizada);

        File.WriteAllText(arquivo, json);

        return 0;
    }
}

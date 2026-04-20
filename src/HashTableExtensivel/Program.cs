using HashTableExtensivel.Cli.Comandos;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.PropagateExceptions();
    config
        .AddCommand<CriarHashTableCommand>("criar")
        .WithDescription("Cria um arquivo JSON representando uma tabela hash extensível.")
        .WithExample(["criar", "-t", "4", "-a", "minha_hash_table.json", "-c", "String"]);
    config
        .AddCommand<AdicionarElementoCommand>("adicionar")
        .WithDescription("Adiciona um elemento a uma tabela hash extensível existente.")
        .WithExample(["adicionar", "-a", "minha_hash_table.json", "-c", "chave1", "-e", "elemento1"]);
    config
        .AddCommand<GerarArquivoDotCommand>("gerar-dot")
        .WithDescription("Gera um arquivo DOT a partir de um arquivo JSON representando uma tabela hash extensível.")
        .WithExample(["gerar-dot", "-a", "minha_hash_table.json", "-o", "minha_hash_table.dot"]);
    config
        .AddCommand<GerarSvgCommand>("gerar-svg")
        .WithDescription("Gera um arquivo SVG a partir de um arquivo DOT representando uma tabela hash extensível.")
        .WithExample(["gerar-svg", "-a", "minha_hash_table.json", "-o", "minha_hash_table.svg"]);
    config
        .AddCommand<RemoverElementoCommand>("remover")
        .WithDescription("Remove um elemento de uma tabela hash extensível existente.")
        .WithExample(["remover", "-a", "minha_hash_table.json", "-c", "chave1"]);
    config
        .AddCommand<BuscarCommand>("buscar")
        .WithDescription("Busca um elemento em uma tabela hash extensível existente.")
        .WithExample(["buscar", "-a", "minha_hash_table.json", "-c", "chave1"]);
});
return app.Run(args);


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
});
return app.Run(args);


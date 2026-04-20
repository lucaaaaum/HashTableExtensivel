// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using HashTableExtensivel.EstruturasDeDados;
using HashTableExtensivel.Graphviz;

Console.WriteLine("Hello, World!");

var hashTable = new HashTable<Pessoa>(2);

for (int i = 0; i < 10; i++)
{
    hashTable.Inserir(new Pessoa($"Pessoa {i}", 31));
    hashTable.GerarArquivoDot($"hash_table_{i}.dot");
    var processoParaIniciarDot = new ProcessStartInfo
    {
        FileName = "dot",
        Arguments = $"-Tpng hash_table_{i}.dot -o hash_table_{i}.png",
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };
    using var processo = Process.Start(processoParaIniciarDot) ?? throw new InvalidOperationException("Não foi possível iniciar o processo para gerar a imagem do gráfico.");
    processo.WaitForExit();
}

public class Pessoa : IChaveável
{
    public int Id { get; set; }
    public string Nome { get; init; }
    public int Idade { get; init; }

    public Pessoa(string nome, int idade)
    {
        Id = GeradorDeId.GerarId();
        Nome = nome;
        Idade = idade;
    }

    public int ObterChave() => Id;
}

public static class GeradorDeId
{
    private static int _contador = 0;

    public static int GerarId() => ++_contador;
}

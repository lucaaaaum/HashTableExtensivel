// See https://aka.ms/new-console-template for more information
using HashTableExtensivel.EstruturasDeDados;
using HashTableExtensivel.Graphviz;

Console.WriteLine("Hello, World!");

var hashTable = new HashTable<Pessoa>(2);

hashTable.Inserir(new Pessoa("Alice", 30));
hashTable.Inserir(new Pessoa("Bob", 25));
hashTable.Inserir(new Pessoa("Charlie", 35));
hashTable.Inserir(new Pessoa("Diana", 28));
hashTable.Inserir(new Pessoa("Eve", 22));

hashTable.GerarArquivoDot("/home/lucaaaum/teste.dot");

public class Pessoa : IChaveável
{
    public int Id { get; set; }
    public string Nome { get; init; }
    public int Idade { get; init; }

    public Pessoa(string nome, int idade)
    {
        Id = new Random().Next(1, 1000);
        Nome = nome;
        Idade = idade;
    }

    public int ObterChave() => Id;
}

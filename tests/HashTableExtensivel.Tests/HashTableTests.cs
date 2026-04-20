using HashTableExtensivel.EstruturasDeDados;

namespace HashTableExtensivel.Tests;

[TestFixture]
public class HashTableTests
{
    [Test]
    public void HashTable_DeveAdicionarValores()
    {
        var hashTable = new HashTable<int, Fruta>(2);
        var frutas = new List<Fruta>
        {
            new("Banana"),
            new("Laranja"),
            new("Abacaxi"),
            new("Uva")
        };
        foreach (var fruta in frutas)
            hashTable.Inserir(fruta.Id, fruta);

        foreach (var fruta in frutas)
        {
            var chave = fruta.ObterChave();
            var frutaEncontrada = hashTable.Buscar(chave);
            Assert.That(frutaEncontrada, Is.Not.Null, $"Fruta '{fruta}' não encontrada na hash table.");
            Assert.That(frutaEncontrada!.Nome, Is.EqualTo(fruta.Nome), $"Fruta encontrada tem nome '{frutaEncontrada.Nome}', esperado '{fruta.Nome}'.");
        }
    }

    [Test]
    public void HashTable_DeveAumentarProfundidadeAoAdicionarMuitosValores()
    {
        var hashTable = new HashTable<int, Fruta>(2);
        var frutas = new List<Fruta>
        {
            new("Banana"),
            new("Laranja"),
            new("Abacaxi"),
            new("Uva"),
            new("Maçã"),
            new("Pera"),
            new("Manga"),
            new("Melancia")
        };
        foreach (var fruta in frutas)
            hashTable.Inserir(fruta.Id, fruta);

        Assert.That(hashTable.Profundidade, Is.EqualTo(2), "A profundidade da hash table deveria ter aumentado após inserir muitos valores.");
    }

    [Test]
    public void HashTable_DeveRemoverValores()
    {
        var hashTable = new HashTable<int, Fruta>(2);
        var frutas = new List<Fruta>
        {
            new("Banana"),
            new("Laranja"),
            new("Abacaxi"),
            new("Uva"),
            new("Maçã"),
            new("Pera"),
            new("Manga"),
            new("Melancia")
        };
        foreach (var fruta in frutas)
            hashTable.Inserir(fruta.Id, fruta);

        var profundidadeAntesDaRemoção = hashTable.Profundidade;
        Assert.That(profundidadeAntesDaRemoção, Is.EqualTo(2), "A profundidade da hash table deveria ter aumentado após inserir muitos valores.");

        foreach (var fruta in frutas)
        {
            var chave = fruta.ObterChave();
            hashTable.Remover(chave);
            var frutaEncontrada = hashTable.Buscar(chave);
            Assert.That(frutaEncontrada, Is.Null, $"Fruta '{fruta}' deveria ter sido removida da hash table.");
        }

        hashTable.Imprimir();

        Assert.That(hashTable.Profundidade, Is.EqualTo(0), "A profundidade da hash table deveria ter diminuído após remover todas as frutas.");
    }
}

public static class GeradorDeId
{
    private static int _contador = 0;

    public static int GerarId() => ++_contador;
}

public class Fruta(string nome) : IChaveável
{
    public int Id { get; init; } = GeradorDeId.GerarId();
    public string Nome { get; set; } = nome;

    public int ObterChave() => Id;
}

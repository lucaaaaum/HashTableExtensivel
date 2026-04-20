using HashTableExtensivel.EstruturasDeDados;

namespace HashTableExtensivel.Tests;

[TestFixture]
public class HashTableTests
{
    [Test]
    public void HashTable_DeveAdicionarValores()
    {
        var hashTable = new HashTable<Fruta>(2);
        var frutas = new List<Fruta>
        {
            new("Banana"),
            new("Laranja"),
            new("Abacaxi"),
            new("Uva")
        };
        foreach (var fruta in frutas)
            hashTable.Inserir(fruta);

        foreach (var fruta in frutas)
        {
            var chave = fruta.ObterChave();
            var frutaEncontrada = hashTable.Buscar(chave);
            Assert.That(frutaEncontrada, Is.Not.Null, $"Fruta '{fruta}' não encontrada na hash table.");
            Assert.That(frutaEncontrada!.Nome, Is.EqualTo(fruta.Nome), $"Fruta encontrada tem nome '{frutaEncontrada.Nome}', esperado '{fruta.Nome}'.");
        }
    }
}

public class Fruta(string nome) : IChaveável
{
    public string Nome { get; set; } = nome;

    public int ObterChave() => Nome.Sum(c => c * 3);
}

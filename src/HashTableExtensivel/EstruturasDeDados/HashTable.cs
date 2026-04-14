namespace HashTableExtensivel.EstruturasDeDados;

public class HashTable<TElemento>
    where TElemento : IChaveável
{
    public int TamanhoDoBucket { get; init; }
    public int Profundidade { get; private set; }
    private Bucket<TElemento>[] Diretório { get; set; }

    public HashTable(int tamanhoDoBucket)
    {
        TamanhoDoBucket = tamanhoDoBucket;
        Profundidade = 0;
        Diretório = new Bucket<TElemento>[Profundidade];
    }

    private void Inserir(TElemento elemento)
    {
        var chave = elemento.ObterChave();
        var hash = chave % (int)Math.Pow(2, Profundidade);
        var bucket = Diretório[hash];
        bucket.Inserir(chave, elemento);
    }

    private TElemento? Buscar(int chave)
    {
        var hash = chave % (int)Math.Pow(2, Profundidade);
        var bucket = Diretório[hash];
        return bucket.Buscar(chave);
    }

    private void Remover(int chave)
    {
        var hash = chave % (int)Math.Pow(2, Profundidade);
        var bucket = Diretório[hash];
        bucket.Remover(chave);
    }
}

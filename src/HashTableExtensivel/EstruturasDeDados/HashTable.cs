using System.Text.Json;
using System.Text.Json.Serialization;

namespace HashTableExtensivel.EstruturasDeDados;

public class HashTable<TElemento> where TElemento : IChaveável
{
    public int TamanhoDoBucket { get; init; }
    public int Profundidade { get; private set; }
    [JsonInclude]
    private Bucket<TElemento>[] Diretório { get; set; }

    public HashTable(int tamanhoDoBucket)
    {
        TamanhoDoBucket = tamanhoDoBucket;
        Profundidade = 0;
        Diretório = new Bucket<TElemento>[1];
        Diretório[0] = new Bucket<TElemento>(Profundidade, TamanhoDoBucket);
    }

    public void Inserir(TElemento elemento)
    {
        var chave = elemento.ObterChave();
        var hash = chave.CalcularHash(Profundidade);

        var bucket = Diretório[hash];

        if (!bucket.Cheio)
        {
            bucket.Inserir(elemento);
            return;
        }

        if (bucket.Profundidade < Profundidade)
        {
            var novoBucket = bucket.Dividir();
            Diretório[hash] = novoBucket;
        }
        else if (bucket.Profundidade == Profundidade)
        {
            var tamanhoDoDiretórioAtual = Diretório.Length;
            Profundidade++;
            var novoBucket = bucket.Dividir();
            var tamanhoDoNovoDiretório = (int)Math.Pow(2, Profundidade);
            var novoDiretório = new Bucket<TElemento>[tamanhoDoNovoDiretório];

            for (int i = 0; i < tamanhoDoDiretórioAtual; i++)
            {
                novoDiretório[i] = Diretório[i];
                novoDiretório[i + tamanhoDoDiretórioAtual] = Diretório[i];
            }

            Diretório = novoDiretório;
            Diretório[hash + tamanhoDoDiretórioAtual] = novoBucket;
        }

        Inserir(elemento);
    }

    public TElemento? Buscar(int chave)
    {
        var hash = chave.CalcularHash(Profundidade);
        var bucket = Diretório[hash];
        return bucket.Buscar(chave);
    }

    public void Remover(int chave)
    {
        var hash = chave.CalcularHash(Profundidade);
        var bucket = Diretório[hash];
        bucket.Remover(chave);
    }

    public void Imprimir()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
        Console.WriteLine(json);
    }

    public Bucket<TElemento>[] ObterDiretório() => Diretório;
}

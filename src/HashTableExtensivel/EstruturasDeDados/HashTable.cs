using System.Text.Json;
using System.Text.Json.Serialization;

namespace HashTableExtensivel.EstruturasDeDados;

public class HashTable<TChave, TElemento>
{
    public int TamanhoDoBucket { get; init; }
    public int Profundidade { get; private set; }
    [JsonInclude]
    private Bucket<TChave, TElemento>[] Diretório { get; set; }

    public HashTable(int tamanhoDoBucket)
    {
        TamanhoDoBucket = tamanhoDoBucket;
        Profundidade = 0;
        Diretório = new Bucket<TChave, TElemento>[1];
        Diretório[0] = new Bucket<TChave, TElemento>(Profundidade, TamanhoDoBucket);
    }

    public void Inserir(TChave chave, TElemento elemento)
    {
        var hash = chave.CalcularHash(Profundidade);

        var bucket = Diretório[hash];

        if (!bucket.Cheio)
        {
            bucket.Inserir(chave, elemento);
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
            var novoDiretório = new Bucket<TChave, TElemento>[tamanhoDoNovoDiretório];

            for (int i = 0; i < tamanhoDoDiretórioAtual; i++)
            {
                novoDiretório[i] = Diretório[i];
                novoDiretório[i + tamanhoDoDiretórioAtual] = Diretório[i];
            }

            Diretório = novoDiretório;
            Diretório[hash + tamanhoDoDiretórioAtual] = novoBucket;
        }

        Inserir(chave, elemento);
    }

    public TElemento? Buscar(TChave chave)
    {
        var hash = chave.CalcularHash(Profundidade);
        var bucket = Diretório[hash];
        return bucket.Buscar(chave);
    }

    public void Remover(TChave chave)
    {
        var hash = chave.CalcularHash(Profundidade);
        var bucket = Diretório[hash];
        bucket.Remover(chave);

        if (bucket.Vazio && Profundidade > 0)
        {
            var outroBucket = Diretório[chave.CalcularHash(Profundidade - 1)];
            Diretório[hash] = outroBucket;
            outroBucket.ReduzirProfundidade();
        }

        if (Diretório.Distinct().All(b => b.Profundidade < Profundidade))
        {
            Profundidade--;
            var tamanhoDoNovoDiretório = (int)Math.Pow(2, Profundidade);
            var novoDiretório = new Bucket<TChave, TElemento>[tamanhoDoNovoDiretório];
            Array.Copy(Diretório, novoDiretório, tamanhoDoNovoDiretório);
            Diretório = novoDiretório;
        }
    }

    public void Imprimir()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
        Console.WriteLine(json);
    }

    public Bucket<TChave, TElemento>[] ObterDiretório() => Diretório;
}

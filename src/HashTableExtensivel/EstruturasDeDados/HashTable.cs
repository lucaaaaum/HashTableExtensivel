using System.Text.Json;
using System.Text.Json.Serialization;

namespace HashTableExtensivel.EstruturasDeDados;

public class HashTable<TElemento>
    where TElemento : IChaveável
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
        var hash = chave % (int)Math.Pow(2, Profundidade);

        Console.WriteLine($"Inserindo elemento com chave {chave} no bucket {hash}");
        var bucket = Diretório[hash];

        if (!bucket.Cheio)
        {
            bucket.Inserir(elemento);
            return;
        }

        var novoBucket = bucket.Dividir();

        if (bucket.Profundidade > Profundidade)
        {
            Profundidade++;

            var tamanhoDoDiretórioAtual = Diretório.Length;
            var tamanhoDoNovoDiretório = (int)Math.Pow(2, Profundidade);
            var novoDiretório = new Bucket<TElemento>[tamanhoDoNovoDiretório];

            for (int i = 0; i < Diretório.Length; i++)
            {
                novoDiretório[i] = Diretório[i];
                novoDiretório[i + tamanhoDoDiretórioAtual] = Diretório[i];
            }

            Diretório = novoDiretório;
        }


        var mascara = (1 << novoBucket.Profundidade) - 1;
        var bitNovo = 1 << (novoBucket.Profundidade - 1);
        for (int i = 0; i < Diretório.Length; i++)
        {
            if (Diretório[i] == bucket && (i & mascara) >= bitNovo)
                Diretório[i] = novoBucket;
        }

        Inserir(elemento);
    }

    public TElemento? Buscar(int chave)
    {
        var hash = chave % (int)Math.Pow(2, Profundidade);
        var bucket = Diretório[hash];
        Console.WriteLine($"Buscando elemento com chave {chave} no bucket {hash}");
        return bucket.Buscar(chave);
    }

    public void Remover(int chave)
    {
        var hash = chave % (int)Math.Pow(2, Profundidade);
        var bucket = Diretório[hash];
        bucket.Remover(chave);
    }

    public void Imprimir()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
        Console.WriteLine(json);
    }
}

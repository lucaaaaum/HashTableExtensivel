using System.Text.Json.Serialization;

namespace HashTableExtensivel.EstruturasDeDados;

public class Bucket<TElemento>(int profundidade, int tamanho)
    where TElemento : IChaveável
{
    public int Profundidade { get; private set; } = profundidade;
    public bool Cheio => Elementos.All(elemento => elemento is not null);
    [JsonInclude]
    private TElemento?[] Elementos { get; init; } = new TElemento[tamanho];
    private int Tamanho { get; init; } = tamanho;

    public void Inserir(TElemento elemento)
    {

        for (int i = 0; i < Tamanho; i++)
            if (Elementos[i] is null)
            {
                Elementos[i] = elemento;
                return;
            }

        throw new InvalidOperationException("Não foi possível inserir o elemento no bucket.");
    }

    public TElemento? Buscar(int chave)
    {
        foreach (var elemento in Elementos)
        {
            if (elemento is null)
                continue;
            if (elemento.ObterChave() == chave)
                return elemento;
        }
        return default;
    }

    public void Remover(int chave)
    {
        for (int i = 0; i < Tamanho; i++)
        {
            if (Elementos[i] is null)
                continue;
            if (Elementos[i]!.ObterChave() == chave)
            {
                Elementos[i] = default;
                break;
            }
        }

        throw new InvalidOperationException("Elemento não encontrado para remoção.");
    }

    public Bucket<TElemento> Dividir()
    {
        if (!Cheio)
            throw new InvalidOperationException("Não é possível dividir um bucket que não está cheio.");

        Profundidade++;
        var novoBucket = new Bucket<TElemento>(Profundidade, Tamanho);
        for (int i = 0; i < Tamanho; i++)
        {
            var elemento = Elementos[i]!;
            var chave = elemento.ObterChave();
            var bit = (chave >> (Profundidade - 1)) & 1;
            if (bit == 0)
                continue;
            novoBucket.Inserir(elemento);
            Elementos[i] = default;
        }

        return novoBucket;
    }

    public TElemento?[] ObterElementos() => Elementos;
}

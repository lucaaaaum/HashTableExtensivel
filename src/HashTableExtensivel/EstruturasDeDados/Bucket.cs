using System.Text.Json.Serialization;

namespace HashTableExtensivel.EstruturasDeDados;

public class Bucket<TChave, TElemento>(int profundidade, int tamanho)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public int Profundidade { get; private set; } = profundidade;
    public bool Cheio => Elementos.All(elemento => elemento is not null);
    public bool Vazio => Elementos.All(elemento => elemento is null);
    [JsonInclude]
    private (TChave chave, TElemento elemento)?[] Elementos { get; init; } = new (TChave, TElemento)?[tamanho];
    private int Tamanho { get; init; } = tamanho;

    public void Inserir(TChave chave, TElemento elemento)
    {
        for (int i = 0; i < Tamanho; i++)
            if (Elementos[i] is null)
            {
                Elementos[i] = (chave, elemento);
                return;
            }
            else if (Elementos[i]!.Value.chave!.Equals(chave))
                throw new InvalidOperationException("Não é possível inserir um elemento com chave duplicada.");

        throw new InvalidOperationException("Não foi possível inserir o elemento no bucket.");
    }

    public TElemento? Buscar(int chave)
    {
        foreach (var elemento in Elementos)
        {
            if (elemento is null)
                continue;
            if (elemento!.Value.chave!.Equals(chave))
                return elemento!.Value.elemento;
        }
        return default;
    }

    public void Remover(int chave)
    {
        for (int i = 0; i < Tamanho; i++)
        {
            if (Elementos[i] is null)
                continue;
            if (Elementos[i]!.Value.chave!.Equals(chave))
            {
                Elementos[i] = default;
                return;
            }
        }

        throw new InvalidOperationException("Elemento não encontrado para remoção.");
    }

    public void ReduzirProfundidade()
    {
        if (Profundidade == 0)
            throw new InvalidOperationException("Não é possível reduzir a profundidade de um bucket com profundidade zero.");

        Profundidade--;
    }

    public Bucket<TChave, TElemento> Dividir()
    {
        if (!Cheio)
            throw new InvalidOperationException("Não é possível dividir um bucket que não está cheio.");

        Profundidade++;

        var novoBucket = new Bucket<TChave, TElemento>(Profundidade, Tamanho);
        for (int i = 0; i < Tamanho; i++)
        {
            var (chave, elemento) = Elementos[i]!.Value;
            var hashAtual = chave.CalcularHash(Profundidade - 1);
            var hashNovo = chave.CalcularHash(Profundidade);
            if (hashAtual == hashNovo)
                continue;
            novoBucket.Inserir(chave, elemento);
            Elementos[i] = default;
        }

        for (int i = 0; i < Tamanho; i++)
        {
            if (Elementos[i] is not null)
                continue;

            for (int j = i; j < Tamanho; j++)
            {
                var elemento = Elementos[j];
                if (elemento is null)
                    continue;
                Elementos[i] = elemento;
                Elementos[j] = default;
            }
        }

        return novoBucket;
    }

    public (TChave chave, TElemento elemento)?[] ObterChavesEElementos() => [.. Elementos];

    public TElemento?[] ObterElementos() => [.. Elementos.Select(e => e!.Value.elemento)];
}

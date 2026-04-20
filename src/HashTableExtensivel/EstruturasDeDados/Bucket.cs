using System.Text.Json.Serialization;

namespace HashTableExtensivel.EstruturasDeDados;

public class Bucket<TElemento>(int profundidade, int tamanho)
    where TElemento : IChaveável
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public int Profundidade { get; private set; } = profundidade;
    public bool Cheio => Elementos.All(elemento => elemento is not null);
    public bool Vazio => Elementos.All(elemento => elemento is null);
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
            else if (Elementos[i]!.ObterChave() == elemento.ObterChave())
                throw new InvalidOperationException("Não é possível inserir um elemento com chave duplicada.");

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
            var hashAtual = chave.CalcularHash(Profundidade - 1);
            var hashNovo = chave.CalcularHash(Profundidade);
            if (hashAtual == hashNovo)
                continue;
            novoBucket.Inserir(elemento);
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

    public TElemento?[] ObterElementos() => Elementos;
}

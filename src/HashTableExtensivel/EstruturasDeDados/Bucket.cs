namespace HashTableExtensivel.EstruturasDeDados;

public class Bucket<TElemento>(int profundidade, int tamanho)
    where TElemento : IChaveável
{
    public int Profundidade { get; private set; } = profundidade;
    public bool Cheio => QuantidadeDeElementos == Tamanho;
    private TElemento?[] Elementos { get; init; } = new TElemento[tamanho];
    private int QuantidadeDeElementos { get; set; } = 0;
    private int Tamanho { get; init; } = tamanho;

    public void Inserir(int chave, TElemento elemento)
    {
        if (Cheio)
            throw new InvalidOperationException("O bucket já está cheio.");
        var hash = chave % (int)Math.Pow(2, Profundidade);
        Elementos[hash] = elemento;
        QuantidadeDeElementos++;
    }

    public TElemento Buscar(int chave)
    {
        var hash = chave % (int)Math.Pow(2, Profundidade);
        return Elementos[hash];
    }

    public void Remover(int chave)
    {
        var hash = chave % (int)Math.Pow(2, Profundidade);
        Elementos[hash] = default;
        QuantidadeDeElementos--;
    }
}

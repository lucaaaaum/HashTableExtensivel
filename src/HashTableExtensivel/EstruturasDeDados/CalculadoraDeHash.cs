namespace HashTableExtensivel.EstruturasDeDados;

public static class CalculadoraDeHash
{
    public static int CalcularHash<TChave>(this TChave chave, int profundidade)
    {
        if (chave is int chaveInt)
            return CalcularHash(chaveInt, profundidade);

        if (chave is string chaveString)
            return CalcularHash(chaveString, profundidade);

        if (chave is IChaveável chaveChaveável)
            return CalcularHash(chaveChaveável.ObterChave(), profundidade);

        throw new InvalidOperationException("Tipo de chave não suportado para cálculo de hash.");
    }

    private static int CalcularHash(int chave, int profundidade) =>
        chave % (int)Math.Pow(2, profundidade);

    private static int CalcularHash(string chave, int profundidade)
    {
        var chaveInt = ObterIntDeChaveString(chave);
        return CalcularHash(chaveInt, profundidade);
    }

    private static int ObterIntDeChaveString(string chave) =>
        chave.Aggregate(17, (hash, c) => hash * 31 + c);
}

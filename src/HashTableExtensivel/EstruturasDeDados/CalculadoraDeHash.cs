namespace HashTableExtensivel.EstruturasDeDados;

public static class CalculadoraDeHash
{
    public static int CalcularHash(this int chave, int profundidade) =>
        chave % (int)Math.Pow(2, profundidade);
}

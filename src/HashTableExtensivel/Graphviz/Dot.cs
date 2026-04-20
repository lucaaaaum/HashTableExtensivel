using System.Diagnostics;

namespace HashTableExtensivel.Graphviz;

public static class Dot
{
    public static void GerarArquivoSvg(string caminhoDoArquivoDot, string caminhoDoArquivoSvg)
    {
        var processoParaIniciarDot = new ProcessStartInfo
        {
            FileName = "dot",
            Arguments = $"-Tsvg {caminhoDoArquivoDot} -o {caminhoDoArquivoSvg}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var processo = Process.Start(processoParaIniciarDot) ?? throw new InvalidOperationException("Não foi possível iniciar o processo para gerar a imagem do gráfico.");
        processo.WaitForExit();
    }
}

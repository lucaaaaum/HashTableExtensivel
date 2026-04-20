using System.Text;
using HashTableExtensivel.EstruturasDeDados;

namespace HashTableExtensivel.Graphviz;

public static class GeradorDeArquivoDot
{
    public static void GerarArquivoDot<TElemento>(this HashTable<TElemento> hashTable, string caminhoDoArquivo)
        where TElemento : IChaveável
    {
        var diretório = hashTable.ObterDiretório();
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("digraph HashTable {");
        stringBuilder.AppendLine("    node [shape=plaintext];");
        stringBuilder.AppendLine("    rankdir=LR;");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("    diretorio [label=<");
        stringBuilder.AppendLine("        <TABLE BORDER=\"1\" CELLBORDER=\"1\" CELLSPACING=\"0\">");
        stringBuilder.AppendLine($"            <TR><TD COLSPAN=\"1\">Profundidade: {hashTable.Profundidade}</TD></TR>");
        for (int i = 0; i < diretório.Length; i++)
        {
            var hash = Convert.ToString(i, 2).PadLeft(hashTable.Profundidade, '0');
            stringBuilder.AppendLine($"            <TR><TD PORT=\"diretorio{i}\">{hash}</TD></TR>");
        }
        stringBuilder.AppendLine("       </TABLE>");
        stringBuilder.AppendLine("    >];");
        stringBuilder.AppendLine();

        var bucketsIds = diretório.Distinct().Select((bucket, índice) => (bucket, índice)).ToList();
        foreach ((var bucket, var id) in bucketsIds)
        {
            stringBuilder.AppendLine($"    bucket{id} [label=<");
            stringBuilder.AppendLine("        <TABLE BORDER=\"1\" CELLBORDER=\"1\" CELLSPACING=\"0\">");
            stringBuilder.AppendLine($"            <TR><TD COLSPAN=\"2\">Bucket {id}: Profundidade = {bucket.Profundidade}</TD></TR>");
            stringBuilder.AppendLine("            <TR><TD>Chave</TD><TD>Elemento</TD></TR>");
            foreach (var elemento in bucket.ObterElementos())
            {
                if (elemento is null)
                    stringBuilder.AppendLine("            <TR><TD></TD><TD></TD></TR>");
                else
                    stringBuilder.AppendLine($"            <TR><TD>{elemento.ObterChave()}</TD><TD>{elemento}</TD></TR>");
            }
            stringBuilder.AppendLine("        </TABLE>");
            stringBuilder.AppendLine("    >];");
        }

        stringBuilder.AppendLine();

        for (int i = 0; i < diretório.Length; i++)
        {
            var bucket = diretório[i];
            var id = bucketsIds.FirstOrDefault(bucketId => bucketId.bucket == bucket).índice;
            stringBuilder.AppendLine($"    diretorio:diretorio{i} -> bucket{id}");
        }

        stringBuilder.AppendLine("}");

        File.WriteAllText(caminhoDoArquivo, stringBuilder.ToString());
    }
}

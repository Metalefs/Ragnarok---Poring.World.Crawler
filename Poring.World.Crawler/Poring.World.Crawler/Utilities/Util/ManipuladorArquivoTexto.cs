using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Utilities.Files
{
    public static class ManipuladorArquivoTexto
    {
        public static void EscreverArquivo(string texto, string path)
        {
            using (TextWriter tw = new StreamWriter(path, true))
            {
                tw.WriteLine(texto);
            }
        }

        public static void EscreverArquivoAntigo(string texto, string path)
        {
            if (File.Exists(path))
            {
                texto = File.ReadAllText(path) + texto;
            }

            using (TextWriter tw = new StreamWriter(path))
            {
                tw.WriteLine(texto);
            }
        }
    }
}

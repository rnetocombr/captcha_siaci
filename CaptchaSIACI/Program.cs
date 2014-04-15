using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Collections;
using System.Collections.Specialized;

namespace CaptchaSIACI
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CaminhoDicionario = @"dicionario.txt";
            const string CaminhoTela = @"tela.txt";

            foreach (string arquivo in Directory.GetFiles(".", "*.txt"))
            {
                Console.WriteLine(arquivo);
                if (arquivo.Contains("tela"))
                {
                    CaptchaSIACI captchaSIACI = new CaptchaSIACI(CaminhoDicionario);
                    List<string> resultado = captchaSIACI.QuebraCaptcha(arquivo);

                    string resultadoFormatado = "";
                    foreach (var chave in resultado)
                    {
                        resultadoFormatado += chave;
                    }

                    Console.Clear();

                    IEnumerable<string> linhas = File.ReadLines(arquivo);
                    foreach (string linha in linhas)
                    {
                        Console.Write(linha);
                    }

                    Console.WriteLine("Arquivo: " + arquivo + " - Captcha: " + resultadoFormatado);

                    Console.ReadLine();
                }
            }

            

            Console.WriteLine("fim...");
            Console.ReadLine();
        }
    }
}

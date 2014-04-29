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
            CaptchaSIACI captchaSIACI = new CaptchaSIACI(CaminhoDicionario);

            foreach (string arquivo in Directory.GetFiles(".", "*.txt"))
            {
                if (arquivo.Contains("tela"))
                Console.WriteLine(arquivo + "-" + captchaSIACI.QuebraCaptcha(arquivo));
            }

            Console.ReadLine();
        }
    }
}

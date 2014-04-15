using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

namespace CaptchaSIACI
{
    class CaptchaSIACI
    {
        List<KeyValuePair<string, List<string>>> representacoes = new List<KeyValuePair<string, List<string>>>();

        public CaptchaSIACI(string caminhoDoDicionario) {
            // Inicializa o dicionário
            // string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Carregando o dicionário
            List<string> buffer = new List<string>();

            // Preenchendo com as representações
            foreach (string linha in File.ReadAllLines(caminhoDoDicionario))
            {
                if (linha.Trim() == String.Empty) continue;

                if (linha.Contains("#"))
                {
                    buffer.Add(linha.TrimEnd() + " ");
                }
                else
                {
                    KeyValuePair<string, List<string>> representacao = new KeyValuePair<string, List<string>>(linha.Trim().ToUpper(), buffer);
                    representacoes.Add(representacao);

                    buffer = new List<string>();
                }
            }

            // Ordenando pelo número de jogos das velhas, quanto mais, maior prioridade.
            representacoes.Sort(
                delegate(KeyValuePair<string, List<string>> rep1, KeyValuePair<string, List<string>> rep2)
                {
                    int totalRep1 = 0;
                    int totalRep2 = 0;

                    foreach (string linha in rep1.Value)
                    {
                        totalRep1 += linha.Count(c => c == '#');
                    }

                    foreach (string linha in rep2.Value)
                    {
                        totalRep2 += linha.Count(c => c == '#');
                    }

                    return totalRep2.CompareTo(totalRep1);
                }
            );

            // Imprime os captchas do dicionário
            // ImprimeCaptchas();
            // Console.ReadLine();
        }

        public void ImprimeCaptchas()
        {
            // Para cada caractere
            foreach (KeyValuePair<string, List<string>> representacao in representacoes)
            {
                Thread.Sleep(500);
                Console.Clear();

                // Imprime o caractere chave
                Console.WriteLine(representacao.Key);
                representacao.Value.ForEach(Console.WriteLine);
            }
        }


        public List<string> QuebraCaptcha(string arquivo)
        {
            // Lista que hospedará o resultado
            List<string> resultado = new List<string>();

            List<string> linhas = new List<string>(File.ReadAllLines(arquivo));
            
            // Trata todas as linhas para que tenham 80 colunas
            IEnumerable<string> linhasTratadas = linhas.Select(f => f.PadRight(80, ' '));

            // Vamos de coluna a coluna, comparando.
            for (int contadorColuna = 0; contadorColuna < 80; contadorColuna++)
            {
                // Para cada representacao...
                foreach (KeyValuePair<string, List<string>> parLetraRepresentacao in representacoes)
                {
                    // Fazemos uma copia das linhas. Vamos ir removendo linhas no caminho.
                    List<string> copiaDasLinhas = new List<string>(linhasTratadas);

                    // Separamos letra e representacao
                    string letra = parLetraRepresentacao.Key;
                    List<string> representacao = parLetraRepresentacao.Value;

                    // Enquanto o número de linhas da cópia do arquivo for maior que a representação
                    while (copiaDasLinhas.Count() > representacao.Count())
                    {
                        // Presumimos que cruzou = verdadeiro
                        bool cruzou = true;

                        // Vamos agora comparando linha a linha
                        for (int contadorLinha = 0; contadorLinha < representacao.Count(); contadorLinha++)
                        {
                            string linhaArquivo = copiaDasLinhas[contadorLinha];
                            string linhaRepresentacao = representacao[contadorLinha];

                            // Se a linha do arquivo for menor que contadorColuna e o tamanho da representação, pula
                            if (linhaArquivo.Length < (contadorColuna + linhaRepresentacao.Length))
                            {
                                cruzou = false;
                                break;
                            }

                            // Ajusta a linha do arquivo. Para começar da coluna correta e ter o mesmo tamanho que a representação.
                            linhaArquivo = linhaArquivo.Substring(contadorColuna, linhaRepresentacao.Length);

                            if (linhaArquivo != linhaRepresentacao)
                            {
                                cruzou = false;
                                break;
                            }
                        }

                        if (cruzou)
                        {
                            // Adiciona para string resultado
                            resultado.Add(letra);

                            // Incrementa contadorColuna pelo tamanho da maior linha da representação. Evita duplicações.
                            contadorColuna += representacao.OrderByDescending(s => s.Length).First().Length - 1;
                            break;
                        }

                        copiaDasLinhas.RemoveAt(0);
                    }
                }
            }

            return resultado;
        }
    }
}

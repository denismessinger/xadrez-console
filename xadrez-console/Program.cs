using System;
using tabuleiro;
using xadrez_console;
using xadrez;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PosicaoXadrez p = new PosicaoXadrez('a', 1);

                Console.WriteLine(p.toPosicao());
            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
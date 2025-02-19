using System;
using tabuleiro;
using xadrez_console;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tabuleiro p = new Tabuleiro(8, 8);

            Tela.imprimirTabuleiro(p);
        }
    }
}
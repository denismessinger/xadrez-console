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
            Tabuleiro p = new Tabuleiro(8, 8);
            p.colocarPeca(new Torre(Cor.Preta, p), new Posicao(0, 1));
            Tela.imprimirTabuleiro(p);
        }
    }
}
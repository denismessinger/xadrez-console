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
                PartidaDeXadres partida = new PartidaDeXadres();   
                while (!partida.terminada)
                {
                    
                    Tela.imprimirTabuleiro(partida.tab);

                    Console.Write("Digite a posição de origem: ");
                    Posicao origem = Tela.lerPosicaoXadrez().toPosicao();

                    bool[,] posicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();
                    Console.Clear();
                    Tela.imprimirTabuleiro(partida.tab, posicoesPossiveis);

                    Console.Write("Digite a posição de destino: ");
                    Posicao destino = Tela.lerPosicaoXadrez().toPosicao();

                    partida.executaMovimento(origem, destino);
                }
                

            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
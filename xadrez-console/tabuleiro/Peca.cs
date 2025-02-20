﻿namespace tabuleiro
{
    abstract class Peca
    {
        public Posicao posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int qtdMovimentos { get; protected set; }
        public Tabuleiro tab { get; protected set; }

        public Peca(Cor cor, Tabuleiro tab)
        {
            this.posicao = null;
            Cor = cor;
            this.tab = tab;
            qtdMovimentos = 0;
        }

        public void incrementarQteMovimentos()
        {
            qtdMovimentos++;
        }

        public abstract bool[,] movimentosPossiveis();
    }
}

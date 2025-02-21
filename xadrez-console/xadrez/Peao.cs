using tabuleiro;

namespace xadrez
{
    internal class Peao : Peca
    {
        private PartidaDeXadrez partida;

        public Peao(Cor cor, Tabuleiro tab, PartidaDeXadrez partida) : base(cor, tab)
        {
            this.partida = partida;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p == null || p.Cor != Cor;
        }

        public bool existeInimigo(Posicao pos)
        {
            Peca peca = tab.peca(pos);
            return peca != null && peca.Cor != Cor;
        }

        public bool livre(Posicao pos)
        {
            return tab.peca(pos) == null;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.linhas, tab.colunas];
            Posicao pos = new Posicao(0, 0);

            if (Cor == Cor.Branca)
            {
                // ACIMA
                pos.definirValores(posicao.linha + 1, posicao.coluna);
                if (tab.posicaoValida(pos) && livre(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                // ACIMA PRIMEIRA JOGADA
                pos.definirValores(posicao.linha + 2, posicao.coluna);
                if (tab.posicaoValida(pos) && livre(pos) && qtdMovimentos==0)
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                // ACIMA DIAGONAL ESQUERDA
                pos.definirValores(posicao.linha + 1, posicao.coluna - 1);
                if (tab.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                // ACIMA DIAGONAL DIREITA
                pos.definirValores(posicao.linha + 1, posicao.coluna + 1);
                if (tab.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                // JOGADA ESPECIAL ENPASSANT
                if (posicao.linha == 4)
                {
                    Posicao esquerda = new Posicao(posicao.linha, posicao.coluna - 1);
                    if (tab.posicaoValida(esquerda) && existeInimigo(esquerda) && tab.peca(esquerda) == partida.vulneravelEnPassant)
                    {
                        mat[esquerda.linha + 1, esquerda.coluna] = true;
                    }
                    Posicao direita = new Posicao(posicao.linha, posicao.coluna + 1);
                    if (tab.posicaoValida(direita) && existeInimigo(direita) && tab.peca(direita) == partida.vulneravelEnPassant)
                    {
                        mat[direita.linha + 1, direita.coluna] = true;
                    }
                }
            }
            else
            {
                // ABAIXO
                pos.definirValores(posicao.linha - 1, posicao.coluna);
                if (tab.posicaoValida(pos) && livre(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                // ABAIXO PRIMEIRA JOGADA
                pos.definirValores(posicao.linha - 2, posicao.coluna);
                if (tab.posicaoValida(pos) && livre(pos) && qtdMovimentos == 0)
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                // ABAIXO DIAGONAL ESQUERDA
                pos.definirValores(posicao.linha - 1, posicao.coluna - 1);
                if (tab.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                // ABAIXO DIAGONAL DIREITA
                pos.definirValores(posicao.linha - 1, posicao.coluna + 1);
                if (tab.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                // JOGADA ESPECIAL ENPASSANT
                if (posicao.linha == 3)
                {
                    Posicao esquerda = new Posicao(posicao.linha, posicao.coluna - 1);
                    if (tab.posicaoValida(esquerda) && existeInimigo(esquerda) && tab.peca(esquerda) == partida.vulneravelEnPassant)
                    {
                        mat[esquerda.linha - 1, esquerda.coluna] = true;
                    }
                    Posicao direita = new Posicao(posicao.linha, posicao.coluna + 1);
                    if (tab.posicaoValida(direita) && existeInimigo(direita) && tab.peca(direita) == partida.vulneravelEnPassant)
                    {
                        mat[direita.linha - 1, direita.coluna] = true;
                    }
                }
            }
            
            return mat;
        }
    }
}

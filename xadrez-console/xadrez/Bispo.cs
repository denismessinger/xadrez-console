using tabuleiro;

namespace xadrez
{
    internal class Bispo : Peca
    {
        public Bispo(Cor cor, Tabuleiro tab) : base(cor, tab)
        {

        }

        public override string ToString()
        {
            return "B";
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p == null || p.Cor != Cor;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.linhas, tab.colunas];
            Posicao pos = new Posicao(0, 0);
            // DIAGONAL NOROESTE
            pos.definirValores(posicao.linha + 1, posicao.coluna - 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.linha = pos.linha + 1;
                pos.coluna = pos.coluna - 1;
            }
            // DIAGONAL NORDESTE
            pos.definirValores(posicao.linha + 1, posicao.coluna + 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.linha = pos.linha + 1;
                pos.coluna = pos.coluna + 1;
            }
            // DIAGONAL SUDOESTE
            pos.definirValores(posicao.linha - 1, posicao.coluna - 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.linha = pos.linha - 1;
                pos.coluna = pos.coluna - 1;
            }
            // DIAGONAL SUDESTE
            pos.definirValores(posicao.linha - 1, posicao.coluna + 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.linha = pos.linha - 1;
                pos.coluna = pos.coluna + 1;
            }
            return mat;
        }
    }
}

using tabuleiro;

namespace xadrez
{
    internal class Torre : Peca
    {
        public Torre(Cor cor, Tabuleiro tab) : base(cor, tab)
        {

        }

        public override string ToString()
        {
            return "T";
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
            // ACIMA
            pos.definirValores(posicao.linha - 1, posicao.coluna);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.linha = pos.linha - 1;
            }
            // BAIXO
            pos.definirValores(posicao.linha + 1, posicao.coluna);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.linha = pos.linha + 1;
            }
            // ESQUERDA
            pos.definirValores(posicao.linha, posicao.coluna - 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.coluna = pos.coluna - 1;
            }
            // DIREITA
            pos.definirValores(posicao.linha, posicao.coluna + 1);
            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.coluna = pos.coluna + 1;
            }
            return mat;
        }
    }
}

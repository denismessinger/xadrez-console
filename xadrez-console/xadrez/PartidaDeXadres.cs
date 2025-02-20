using tabuleiro;

namespace xadrez
{
    internal class PartidaDeXadres
    {
        public Tabuleiro tab { get; private set; }
        private int turno;
        private Cor jogadorAtual;
        public bool terminada { get; set; }

        public PartidaDeXadres()
        {
            this.tab = new Tabuleiro(8,8);
            this.turno = 1;
            this.jogadorAtual = Cor.Branca;
            colocarPecas();
            terminada = false;
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
        }

        private void colocarPecas()
        {
            tab.colocarPeca(new Rei(Cor.Preta, tab), new PosicaoXadrez('c', 1).toPosicao());
        }
    }
}

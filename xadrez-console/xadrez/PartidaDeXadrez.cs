using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        public bool xeque { get; private set; }
        public Peca vulneravelEnPassant { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public PartidaDeXadrez()
        {
            this.tab = new Tabuleiro(8,8);
            this.turno = 1;
            this.jogadorAtual = Cor.Preta;
            vulneravelEnPassant = null;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();  
            colocarPecas();
            terminada = false;
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            // JOGADA ESPECIAL ROQUE PEQUENO
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }
            // JOGADA ESPECIAL ROQUE GRANDE
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }
            // JOGADA ESPECIAL ENPASSANT
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(destino.linha - 1, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.linha + 1, destino.coluna);
                    }
                    pecaCapturada = tab.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }
            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca capturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQteMovimentos();
            if (capturada != null)
            {
                tab.colocarPeca(capturada, destino);
                capturadas.Remove(capturada);
            }
            tab.colocarPeca(p, origem);

            // JOGADA ESPECIAL ROQUE PEQUENO
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // JOGADA ESPECIAL ROQUE GRANDE
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retirarPeca(destinoT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }
            // JOGADA ESPECIAL ENPASSANT
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && capturada == vulneravelEnPassant)
                {
                    Peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(4, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(3, destino.coluna);
                    }
                    tab.colocarPeca(peao, posP);
                }
            }
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);
            if (estaEmCheque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Jogada inválida, ficará em cheque!");
            }

            Peca p = tab.peca(destino);
            if (p is Peao)
            {
                if ((p.Cor == Cor.Branca && destino.linha == 7) || (p.Cor == Cor.Preta && destino.linha == 0))
                {
                    p = tab.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca rainha = new Rainha(p.Cor, tab);
                    tab.colocarPeca(rainha, destino);
                    pecas.Add(rainha);
                }
            }

            if (estaEmCheque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            { 
                xeque = false;
            }

            if (testeXequemate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();
            }

            

            // EnPassant
            if (p is Peao && (destino.linha == origem.linha + 2 || destino.linha == origem.linha - 2))
            {
                vulneravelEnPassant = p;
            }
            else
            {
                vulneravelEnPassant = null;
            }
        }

        

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição escolhida!");
            }
            if (jogadorAtual != tab.peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não existem movimentos possíveis para essa peça!");
            }
        }

        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).movimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        public void mudaJogador()
        {
            if (jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Preta;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }
        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Preta)
            {
                return Cor.Branca;
            }
            else
            {
                return Cor.Preta;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool estaEmCheque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não tem mais um rei em jogo!");
            }
            foreach (Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequemate(Cor cor)
        {
            if (!estaEmCheque(cor))
            {
                return false;
            }
            foreach (Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < tab.linhas; i++)
                {
                    for (int j = 0; j < tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao destino = new Posicao(i, j);
                            Peca capturada = executaMovimento(x.posicao, new Posicao(i, j));
                            bool testeXeque = estaEmCheque(cor);
                            desfazMovimento(x.posicao, destino, capturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas()
        {
            colocarNovaPeca('a', 1, new Torre(Cor.Preta, tab));
            colocarNovaPeca('b', 1, new Cavalo(Cor.Preta, tab));
            colocarNovaPeca('c', 1, new Bispo(Cor.Preta, tab));
            colocarNovaPeca('d', 1, new Rainha(Cor.Preta, tab));
            colocarNovaPeca('e', 1, new Rei(Cor.Preta, tab, this));
            colocarNovaPeca('f', 1, new Bispo(Cor.Preta, tab));
            colocarNovaPeca('g', 1, new Cavalo(Cor.Preta, tab));
            colocarNovaPeca('h', 1, new Torre(Cor.Preta, tab));
            colocarNovaPeca('a', 2, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('b', 2, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('c', 2, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('d', 2, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('e', 2, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('f', 2, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('g', 2, new Peao(Cor.Preta, tab, this));
            colocarNovaPeca('h', 2, new Peao(Cor.Preta, tab, this));

            colocarNovaPeca('a', 8, new Torre(Cor.Branca, tab));
            colocarNovaPeca('b', 8, new Cavalo(Cor.Branca, tab));
            colocarNovaPeca('c', 8, new Bispo(Cor.Branca, tab));
            colocarNovaPeca('d', 8, new Rainha(Cor.Branca, tab));
            colocarNovaPeca('e', 8, new Rei(Cor.Branca, tab, this));
            colocarNovaPeca('f', 8, new Bispo(Cor.Branca, tab));
            colocarNovaPeca('g', 8, new Cavalo(Cor.Branca, tab));
            colocarNovaPeca('h', 8, new Torre(Cor.Branca, tab));
            colocarNovaPeca('a', 7, new Peao(Cor.Branca, tab, this));
            colocarNovaPeca('b', 7, new Peao(Cor.Branca, tab, this));
            colocarNovaPeca('c', 7, new Peao(Cor.Branca, tab, this));
            colocarNovaPeca('d', 7, new Peao(Cor.Branca, tab, this));
            colocarNovaPeca('e', 7, new Peao(Cor.Branca, tab, this));
            colocarNovaPeca('f', 7, new Peao(Cor.Branca, tab, this));
            colocarNovaPeca('g', 7, new Peao(Cor.Branca, tab, this));
            colocarNovaPeca('h', 7, new Peao(Cor.Branca, tab, this));
        }
    }
}

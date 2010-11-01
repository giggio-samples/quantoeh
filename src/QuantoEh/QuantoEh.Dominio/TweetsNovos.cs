using System;
using System.Collections.Generic;

namespace QuantoEh.Dominio
{
    public class TweetsNovos
    {
        protected TweetsNovos() {}

        private List<TweetParaProcessar> _novos = new List<TweetParaProcessar>();

        public TweetsNovos(IEnumerable<TweetParaProcessar> novos, ulong ultimoId)
        {
            _novos.AddRange(novos);
            IdMaisAlto = ultimoId;
        }

        public virtual ulong IdMaisAlto { get; private set; }

        public virtual IEnumerable<TweetParaProcessar> Novos { get { return _novos; } }

        public int QuantidadeDeNovos { get { return _novos == null? 0: _novos.Count; } }

        public static TweetsNovos Vazio(ulong ultimoId)
        {
            return new TweetsNovos(new List<TweetParaProcessar>(), ultimoId);
        }
    }
}
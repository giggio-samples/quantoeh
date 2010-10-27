﻿using System.Collections.Generic;

namespace QuantoEh.Dominio
{
    public class TweetsNovos
    {
        protected TweetsNovos() {}

        private List<TweetParaProcessar> _novos;

        public TweetsNovos(IEnumerable<string> textosTweets, ulong ultimoId)
        {
            _novos = new List<TweetParaProcessar>();
            foreach (var textoTweet in textosTweets)
                _novos.Add(new TweetParaProcessar(textoTweet));
            IdMaisAlto = ultimoId;
        }

        public ulong IdMaisAlto { get; private set; }

        public virtual IEnumerable<TweetParaProcessar> Novos { get { return _novos; } }
    }
}
using System;
using System.Collections.Generic;

namespace QuantoEh.Dominio
{
    public class TweetsNovos
    {
        public long IdMaisAlto { get; private set; }

        public virtual IEnumerable<TweetParaProcessar> Novos { get; private set; }
    }
}
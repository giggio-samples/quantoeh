using System;

namespace QuantoEh.Dominio
{
    public class TweetParaProcessar
    {
        public TweetParaProcessar(string textoTweet)
        {
            Texto = textoTweet;
        }

        public string Texto { get; private set; }

        public double Processar()
        {
            return 5;
        }
    }
}
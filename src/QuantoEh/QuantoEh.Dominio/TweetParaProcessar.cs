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
        private readonly TweetParser _tweetParser = new TweetParser();
        private readonly CalculadorDeExpressoes _calculador = new CalculadorDeExpressoes();
        public double Processar()
        {
            var expressao = _tweetParser.CalcularExpressao(Texto);
            var resultado = _calculador.Calcular(expressao);
            return resultado;
        }
    }
}
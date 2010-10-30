using System;
using System.Runtime.Serialization;

namespace QuantoEh.Dominio
{
    [Serializable]
    public class TweetParaProcessar : ISerializable
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

        public override string ToString()
        {
            return Texto;
        }

        public TweetParaProcessar() {}
        public TweetParaProcessar(SerializationInfo info, StreamingContext context) : this()
        {
            Texto = info.GetString("texto");
        }
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("texto", Texto);
        }
    }
}
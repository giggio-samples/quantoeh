using System;
using System.Runtime.Serialization;

namespace QuantoEh.Dominio
{
    [Serializable]
    public class TweetParaProcessar : ISerializable
    {
        
        public TweetParaProcessar(string textoTweet, ulong id)
        {
            Texto = textoTweet;
            Id = id;
        }

        public string Texto { get; private set; }
        public ulong Id { get; private set; }

        private readonly TweetParser _tweetParser = new TweetParser();
        private readonly CalculadorDeExpressoes _calculador = new CalculadorDeExpressoes();
        public Resposta ProcessarResposta()
        {
            var expressao = _tweetParser.CalcularExpressao(Texto);
            var resultado = _calculador.Calcular(expressao);
            var retuite = MontarRetuite(resultado);
            return new Resposta
                       {
                           IdTweetOriginal = Id,
                           Texto = retuite
                       };
        }

        private string MontarRetuite(double resultado)
        {
            return string.Format("{0} RT {1}", resultado, Texto);
        }

        public override string ToString()
        {
            return Texto;
        }

        public TweetParaProcessar() {}
        public TweetParaProcessar(SerializationInfo info, StreamingContext context) : this()
        {
            Texto = info.GetString("texto");
            Id = info.GetUInt64("id");
        }
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("texto", Texto);
            info.AddValue("id", Id);
        }
    }
}
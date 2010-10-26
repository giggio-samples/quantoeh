using System;
using System.Linq;

namespace QuantoEh.Dominio
{
    public class TweetParser
    {
        public string CalcularExpressao(string textoTweet)
        {
            var tokens = textoTweet.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var texto = string.Join(" ", tokens.Skip(2).Select(c => c));
            return texto;
        }
    }
}
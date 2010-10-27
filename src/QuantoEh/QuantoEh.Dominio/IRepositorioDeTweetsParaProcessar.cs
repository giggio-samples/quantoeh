using System.Collections.Generic;

namespace QuantoEh.Dominio
{
    public interface IRepositorioDeTweetsParaProcessar 
    {
        void Adicionar(IEnumerable<TweetParaProcessar> tweetParaProcessar);
    }
}
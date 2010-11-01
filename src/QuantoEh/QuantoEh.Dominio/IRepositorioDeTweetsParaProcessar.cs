using System.Collections.Generic;

namespace QuantoEh.Dominio
{
    public interface IRepositorioDeTweetsParaProcessar 
    {
        void Adicionar(TweetsNovos tweetsNovos);
        IEnumerable<TweetParaProcessar> ObterTodos();
        ulong ObterUltimoId();
    }
}
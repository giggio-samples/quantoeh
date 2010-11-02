using System.Collections.Generic;

namespace QuantoEh.Dominio
{
    public interface ITweetsParaProcessar 
    {
        void Adicionar(TweetsNovos tweetsNovos);
        IEnumerable<TweetParaProcessar> ObterTodos();
        ulong ObterUltimoId();
    }
}
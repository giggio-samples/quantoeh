using System.Collections.Generic;

namespace QuantoEh.Dominio
{
    public interface IRespostasParaRetuitar
    {
        void Adicionar(Resposta resposta);
        IEnumerable<Resposta> ObterTodas();
    }
}

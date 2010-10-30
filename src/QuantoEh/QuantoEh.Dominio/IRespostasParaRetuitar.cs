using System.Collections.Generic;

namespace QuantoEh.Dominio
{
    public interface IRespostasParaRetuitar
    {
        void Adicionar(string textoRetuite);
        IEnumerable<string> ObterTodas();
    }
}

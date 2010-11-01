using System.Collections.Generic;

namespace QuantoEh.Dominio
{
    public interface ITimeline
    {
        int Postar(IEnumerable<Resposta> respostas);
    }
}
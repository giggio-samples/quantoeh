using System.Collections.Generic;

namespace QuantoEh.Dominio
{
    public interface ITimeline
    {
        void Postar(IEnumerable<string> respostas);
    }
}
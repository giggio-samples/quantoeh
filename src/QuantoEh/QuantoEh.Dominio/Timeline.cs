using System;

namespace QuantoEh.Dominio
{
    public interface ITimeline
    {
    }

    public class Timeline : ITimeline
    {
        public bool Contem(string item)
        {
            throw new NotImplementedException();
        }

        public int TotalDeItens { get; private set; }
    }
}
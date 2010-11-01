using System;

namespace QuantoEh.Dominio
{
    [Serializable]
    public class Resposta
    {
        public string Texto { get; set; }
        public ulong IdTweetOriginal { get; set; }        
    }
}
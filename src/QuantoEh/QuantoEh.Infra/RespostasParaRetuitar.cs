using Microsoft.WindowsAzure.StorageClient;
using QuantoEh.Dominio;

namespace QuantoEh.Infra
{
    public class RespostasParaRetuitar :DAOFila, IRespostasParaRetuitar
    {
        public void Adicionar(string textoRetuite)
        {
            var fila = ObterFila();
            fila.AddMessage(new CloudQueueMessage(textoRetuite));
        }
        private static CloudQueue ObterFila()
        {
            var fila = ConfiguracaoArmazenamentoAzure.ObterFila("retweets");
            return fila;
        }

    }
}
using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.StorageClient;
using QuantoEh.Dominio;

namespace QuantoEh.Infra
{
    public class RespostasParaRetuitar :DAOFila, IRespostasParaRetuitar
    {
        public void Adicionar(Resposta resposta)
        {
            var fila = ObterFila();
            var conteudo = Serializar(resposta);
            fila.AddMessage(new CloudQueueMessage(conteudo));
        }

        public IEnumerable<Resposta> ObterTodas()
        {
            throw new NotImplementedException();
        }

        private static CloudQueue ObterFila()
        {
            var fila = ConfiguracaoArmazenamentoAzure.ObterFila("retweets");
            return fila;
        }

    }
}
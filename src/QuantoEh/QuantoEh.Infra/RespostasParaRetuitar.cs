using System;
using System.Collections.Generic;
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

        public IEnumerable<string> ObterTodas()
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
            var fila = ObterFila();
            var mensagem = fila.GetMessage();
            int i = 0;
            while (mensagem != null)
            {
                using (var stream = new MemoryStream(mensagem.AsBytes))
                {
                    var bin = new BinaryFormatter();
                    var resposta = (Resposta)bin.Deserialize(stream);
                    yield return resposta;
                    if (resposta.Processada)
                        fila.DeleteMessage(mensagem);
                }
                if (i >= 10) break;
                i++;
                mensagem = fila.GetMessage();
            }
        }

        private static CloudQueue ObterFila()
        {
            var fila = ConfiguracaoArmazenamentoAzure.ObterFila("retweets");
            return fila;
        }
        public void ResetFila()
        {
            var fila = ObterFila();
            fila.Clear();
        }
    }
}
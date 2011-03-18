using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Trace.TraceInformation("Tweet colocado na fila de resposta: \n{0}", resposta.Texto);
        }

        public IEnumerable<Resposta> ObterTodas()
        {
            var fila = ObterFila();
            var mensagem = fila.GetMessage();
            if (mensagem == null)
            {
                Trace.TraceInformation("Fila de respostas vazia.");
                yield break;
            }
            int i = 0;
            while (mensagem != null)
            {
                using (var stream = new MemoryStream(mensagem.AsBytes))
                {
                    var bin = new BinaryFormatter();
                    var resposta = (Resposta)bin.Deserialize(stream);
                    Trace.TraceInformation("Tweet obtido da fila de resposta: \n{0}", resposta.Texto);
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
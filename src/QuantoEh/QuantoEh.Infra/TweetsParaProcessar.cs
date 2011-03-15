using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.WindowsAzure.StorageClient;
using QuantoEh.Dominio;
using QuantoEh.Infra.EntidadesAzure;

namespace QuantoEh.Infra
{
    public class TweetsParaProcessar : DAOFila, ITweetsParaProcessar
    {
        public void Adicionar(TweetsNovos tweetsNovos)
        {
            ColocarNaFilaDeProcessamento(tweetsNovos.Novos);
            ArmazenarUltimoIdPesquisado(tweetsNovos.IdMaisAlto);
        }

        public void ArmazenarUltimoIdPesquisado(ulong ultimoId)
        {
            var ctx = ConfiguracaoArmazenamentoAzure.ObterTabela("UltimoId");
            var ultimo = ctx.CreateQuery<UltimoId>("UltimoId").FirstOrDefault();
            if (ultimo != null)
            {
                if (ultimo.GetId() == ultimoId)
                {
                    Trace.TraceInformation("Nada a atualizar, o último id {0} se mantém.", ultimoId);
                    return;
                }
                Trace.TraceInformation("Excluindo o ultimo id", ultimo.GetId());
                ctx.DeleteObject(ultimo);
            }
            Trace.TraceInformation("Criando último id para {0}", ultimoId);
            var novoUltimoId = new UltimoId();
            novoUltimoId.SetId(ultimoId);
            ctx.AddObject("UltimoId", novoUltimoId);
            ctx.SaveChanges();
        }

        public ulong ObterUltimoId()
        {
            var ctx = ConfiguracaoArmazenamentoAzure.ObterTabela("UltimoId");
            var ultimo = ctx.CreateQuery<UltimoId>("UltimoId").FirstOrDefault();
            return ultimo == null ? 0 : ultimo.GetId();
        }

        private void ColocarNaFilaDeProcessamento(IEnumerable<TweetParaProcessar> tweetsParaProcessar)
        {
            var fila = ObterFila();
            foreach (var tweet in tweetsParaProcessar)
            {
                var conteudo = Serializar(tweet);
                var mensagem = new CloudQueueMessage(conteudo);
                fila.AddMessage(mensagem);
            }
        }

        public IEnumerable<TweetParaProcessar> ObterTodos()
        {
            var fila = ObterFila();
            var tweets = new List<TweetParaProcessar>();
            var mensagem = fila.GetMessage();
            int i = 0;
            while (mensagem != null)
            {
                using (var stream = new MemoryStream(mensagem.AsBytes))
                {
                    var bin = new BinaryFormatter();
                    var tweet = (TweetParaProcessar)bin.Deserialize(stream);
                    tweets.Add(tweet);
                }
                fila.DeleteMessage(mensagem);
                if (i >= 10) break;
                i++;
                mensagem = fila.GetMessage();
            }
            return tweets;
        }

        private static CloudQueue ObterFila()
        {
            var fila = ConfiguracaoArmazenamentoAzure.ObterFila("tweets");
            return fila;
        }

        public void ResetFila()
        {
            var fila = ObterFila();
            fila.Clear();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.WindowsAzure.StorageClient;
using QuantoEh.Dominio;

namespace QuantoEh.Infra
{
    public class RepositorioDeTweetsParaProcessar : DAOFila, IRepositorioDeTweetsParaProcessar
    {
        public void Adicionar(IEnumerable<TweetParaProcessar> tweetsParaProcessar)
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

    }
}
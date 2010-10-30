using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.WindowsAzure.StorageClient;
using QuantoEh.Dominio;

namespace QuantoEh.Infra
{
    public class RepositorioDeTweetsParaProcessar : IRepositorioDeTweetsParaProcessar
    {
        public void Adicionar(IEnumerable<TweetParaProcessar> tweetsParaProcessar)
        {
            var fila = ObterFila();
            foreach (var tweet in tweetsParaProcessar)
            {
                byte[] conteudo;
                using (var stream = new MemoryStream())
                {
                    var bin = new BinaryFormatter();
                    bin.Serialize(stream, tweet);
                    conteudo = stream.GetBuffer();
                }
                var mensagem = new CloudQueueMessage(conteudo);
                fila.AddMessage(mensagem);
            }
        }

        private static CloudQueue ObterFila()
        {
            var fila = ConfiguracaoArmazenamentoAzure.ObterFila("tweets");
            return fila;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using LinqToTwitter;
using QuantoEh.Dominio;

namespace QuantoEh.Infra
{
    public class DAOTwitter : IMenções, ITimeline
    {
        private static readonly string MyTwitterId = Config.TwitterUserID;
        private static XAuthAuthorizer _autorizadorPin;
        public TweetsNovos ObterNovos(ulong ultimoId)
        {
            var contextoTwitter = ObterContextoTwitter();
            ultimoId = ultimoId <= 0 ? 1 : ultimoId;
            var statuses = (from m in contextoTwitter.Status
                            where m.Type == StatusType.Mentions &
                            m.User.Identifier.UserID != MyTwitterId &
                            m.SinceID == ultimoId
                            select m).ToList();
            if (statuses.Count == 0)
                return TweetsNovos.Vazio(ultimoId);
            var tweets = new List<TweetParaProcessar>();
            foreach (var r in statuses)
            {
                try
                {
                    tweets.Add(new TweetParaProcessar(r.User.Identifier.ScreenName, r.Text, Convert.ToUInt64(r.StatusID)));
                }
                catch (ArgumentException exception)
                {
                    Trace.TraceWarning("Não foi possível colocar tweet na fila de processamento. Erro:\n{0}", exception.ToString());
                }
            }
            var novoUltimoId = statuses.Max(t => Convert.ToUInt64(t.StatusID));
            var tweetsNovos = new TweetsNovos(tweets, novoUltimoId);
            return tweetsNovos;
        }

        public int Postar(IEnumerable<Resposta> respostas)
        {
            var quantidadeDeRespostas = 0;
            var contextoTwitter = ObterContextoTwitter();
            var erros = "";
            foreach (var resposta in respostas)
            {
                try
                {
                    contextoTwitter.UpdateStatus(resposta.Texto, resposta.IdTweetOriginal.ToString());
                    resposta.Processada = true;
                    Trace.TraceInformation("Resposta obtida e retuitada: {0}", resposta.Texto);
                }
                catch (ArgumentException exception)
                {
                    if (exception.ParamName.ToLower() == "status")
                        erros += string.Format("Erro ao tuitar status {0}\n{1}\n", resposta.Texto, exception);
                    else
                        erros += exception + "\n";
                }
                catch (TwitterQueryException exception)
                {
                    if (exception.Response.Error != "Status is a duplicate.")
                    {
                        if (exception.InnerException != null && exception.GetType() == typeof(WebException))
                        {
                            string textoResposta;
                            using (var sr = new StreamReader(((WebException)exception.InnerException).Response.GetResponseStream()))
                            {
                                textoResposta = string.Format("{0}\nResposta:{1}\n", exception, sr.ReadToEnd());
                            }
                            erros += textoResposta;
                        }
                        erros += exception + "\n";
                    }
                    else
                    {
                        resposta.Processada = true;
                    }
                }
                catch (WebException exception)
                {
                    string textoResposta;
                    using (var sr = new StreamReader(exception.Response.GetResponseStream()))
                    {
                        textoResposta = string.Format("{0}\nResposta:{1}\n", exception, sr.ReadToEnd());
                    }
                    erros += textoResposta;
                }
                catch (Exception exception)
                {
                    erros += exception + "\n";
                }

                quantidadeDeRespostas++;
            }
            if (!string.IsNullOrWhiteSpace(erros))
            {
                Trace.TraceError("Erros ao retuitar:\n{0}", erros);
            }
            return quantidadeDeRespostas;
        }

        private static TwitterContext ObterContextoTwitter()
        {
            MontarObjetoDeAutorizacao();
            return new TwitterContext(_autorizadorPin);
        }

        private static void MontarObjetoDeAutorizacao()
        {
            if (_autorizadorPin != null) return;

            var autorizadorPin = new XAuthAuthorizer
            {
                Credentials = new XAuthCredentials
                {
                    ConsumerKey = Config.TwitterConsumerKey,
                    ConsumerSecret = Config.TwitterConsumerSecret,
                    OAuthToken = Config.TwitterOAuthToken,
                    AccessToken = Config.TwitterOAuthTokenSecret,
                    UserName = Config.TwitterScreenName,
                    Password = Config.TwitterPassword,
                }
            };
            try
            {
                autorizadorPin.Authorize();
                _autorizadorPin = autorizadorPin;
            }
            catch (Exception exception)
            {
                Trace.TraceError("Não foi possível autorizar. Erro:\n" + exception.ToString());
                throw;
            }
        }
    }
}

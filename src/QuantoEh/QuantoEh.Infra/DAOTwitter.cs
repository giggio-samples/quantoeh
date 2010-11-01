using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using LinqToTwitter;
using QuantoEh.Dominio;

namespace QuantoEh.Infra
{
    public class DAOTwitter : IMenções, ITimeline
    {
        private static CachedPinAuthorizer _autorizadorPin;
        public TweetsNovos ObterNovos(ulong ultimoId)
        {
            var contextoTwitter = ObterContextoTwitter();
            ultimoId = ultimoId <= 0 ? 1 : ultimoId;
            var statuses = (from m in contextoTwitter.Status
                            where m.Type == StatusType.Mentions &
                            m.SinceID == ultimoId
                            select m).ToList();
            if (statuses.Count == 0)
                return TweetsNovos.Vazio(ultimoId);
            var tweets = (from r in statuses
                          select new TweetParaProcessar(r.User.Identifier.ScreenName + ": " + r.Text, Convert.ToUInt64(r.StatusID))).ToList();
            var novoUltimoId = statuses.Max(t => Convert.ToUInt64(t.StatusID));
            var tweetsNovos = new TweetsNovos(tweets, novoUltimoId);
            return tweetsNovos;
        }

        public void Postar(IEnumerable<Resposta> respostas)
        {
            var contextoTwitter = ObterContextoTwitter();
            foreach (var resposta in respostas)
            {
                contextoTwitter.UpdateStatus(resposta.Texto, resposta.IdTweetOriginal.ToString());
            }
        }

        private static TwitterContext ObterContextoTwitter()
        {
            MontarObjetoDeAutorizacao();
            return new TwitterContext(_autorizadorPin);
        }

        private static void MontarObjetoDeAutorizacao()
        {
            if (_autorizadorPin != null) return;
            _autorizadorPin = new CachedPinAuthorizer
                                  {
                                      ConsumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"],
                                      ConsumerSecret = ConfigurationManager.AppSettings["twitterConsumerSecret"]
                                  };
            _autorizadorPin.MarcarComoAutorizado(
                ConfigurationManager.AppSettings["twitterScreenName"],
                ConfigurationManager.AppSettings["twitterUserID"],
                ConfigurationManager.AppSettings["twitterOAuthToken"],
                ConfigurationManager.AppSettings["twitterOAuthTokenSecret"]);
            //_autorizadorPin.Authorize();
        }
    }
}

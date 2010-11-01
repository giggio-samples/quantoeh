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

            var context = new TwitterContext(_autorizadorPin);
            ultimoId = ultimoId <= 0 ? 1 : ultimoId;
            var statuses = (from m in context.Status
                            where m.Type == StatusType.Mentions &
                            m.SinceID == ultimoId
                            select m).ToList();
            if (statuses.Count == 0)
                return TweetsNovos.Vazio(ultimoId);
            var tweets = (from r in statuses
                          select r.User.Identifier.ScreenName + ": " + r.Text).ToList();
            var novoUltimoId = statuses.Max(t => Convert.ToUInt64(t.StatusID));
            var tweetsNovos = new TweetsNovos(tweets, novoUltimoId);
            return tweetsNovos;
        }

        public void Postar(IEnumerable<string> respostas)
        {
            throw new NotImplementedException();
        }
    }

    public class CachedPinAuthorizer : PinAuthorizer
    {
        public void MarcarComoAutorizado(string screenName, string userId, string oAuthToken, string oAuthTokenSecret)
        {
            ScreenName = screenName;
            UserId = userId;
            OAuthTwitter.OAuthToken = oAuthToken;
            OAuthTwitter.OAuthTokenSecret = oAuthTokenSecret;
            IsAuthorized = true;
        }
        public string ObterLinkParaAutenticacao()
        {
            var link = OAuthTwitter.AuthorizationLinkGet(OAuthRequestTokenUrl, OAuthAuthorizeUrl, "oob", false, false);
            return link;
        }
        public void ObterDadosDeAutenticacao(string link, string pin, out string screenName, out string userId, out string oAuthToken, out string oAuthTokenSecret)
        {
            oAuthToken =
                (from nameValPair in new Uri(link).Query.TrimStart('?').Split('&')
                 let pair = nameValPair.Split('=')
                 where pair[0] == "oauth_token"
                 select pair[1])
                    .SingleOrDefault();
            OAuthTwitter.AccessTokenGet(oAuthToken, pin, OAuthAccessTokenUrl, string.Empty, out screenName, out userId);
            oAuthTokenSecret = OAuthTwitter.OAuthTokenSecret;
        }
    }
}

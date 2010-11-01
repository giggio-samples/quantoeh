using System;
using System.Linq;
using LinqToTwitter;

namespace QuantoEh.Infra
{
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
using NUnit.Framework;
using QuantoEh.Dominio;
using StoryQ.pt_BR;

namespace QuantoEh.Tests.Worker
{
    [TestFixture]
    public class ParsearTweet
    {
        private string _textoTweet;
        private TweetParser _parser;
        private string _expressao;

        [Test]
        public void ParsearTweetTestes()
        {
            new Historia("parsear um tweet")
                .Para("saber quanto da um tweet")
                .Enquanto("usuario do quantoeh")
                .EuQuero("que o quanto eh parseie meus tweets")

                .ComCenario("tweet bem formado com expressao com espaços")
                .Dado(UmTextoDeUmTweet_, "giovannibassi @quantoeh 2 + 3")
                .E(UmParserDeTweets)
                .Quando(ParseioOTweet)
                .Entao(TenhoAExpressaoDaContaIgualA_, "2 + 3")

                .ComCenario("tweet bem formado com expressao sem espaços")
                .Dado(UmTextoDeUmTweet_, "giovannibassi @quantoeh 2+3")
                .E(UmParserDeTweets)
                .Quando(ParseioOTweet)
                .Entao(TenhoAExpressaoDaContaIgualA_, "2+3")
                
                .ComCenario("tweet bem formado com expressao com espaços multiplos")
                .Dado(UmTextoDeUmTweet_, "giovannibassi @quantoeh 2  + 3")
                .E(UmParserDeTweets)
                .Quando(ParseioOTweet)
                .Entao(TenhoAExpressaoDaContaIgualA_, "2 + 3")
 
                .ComCenario("tweet bem formado com expressao com parenteses e multiplas operações")
                .Dado(UmTextoDeUmTweet_, "giovannibassi @quantoeh 5 * (2 + 3)")
                .E(UmParserDeTweets)
                .Quando(ParseioOTweet)
                .Entao(TenhoAExpressaoDaContaIgualA_, "5 * (2 + 3)")

                .Execute();

        }

        private void UmTextoDeUmTweet_(string texto)
        {
            _textoTweet = texto;
        }

        private void UmParserDeTweets()
        {
            _parser = new TweetParser();
        }

        private void ParseioOTweet()
        {
            _expressao = _parser.CalcularExpressao(_textoTweet);
        }

        private void TenhoAExpressaoDaContaIgualA_(string expressaoEsperada)
        {
            Assert.AreEqual(expressaoEsperada, _expressao);

        }
    }
}
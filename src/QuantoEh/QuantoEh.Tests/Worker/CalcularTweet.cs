using System;
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

               .ComCenario("tweet bem formado")
               .Dado(UmTextoDeUmTweetBemFormado)
               .E(UmParserDeTweets)
               .Quando(ParseioOTweet)
               .Entao(TenhoAExpressaoDaConta)

               .Execute();

        }

        private void UmTextoDeUmTweetBemFormado()
        {
            _textoTweet = "giovannibassi @quantoeh 2 + 3"; 
        }

        private void UmParserDeTweets()
        {
            _parser = new TweetParser();
        }

        private void ParseioOTweet()
        {
            _expressao = _parser.CalcularExpressao();
        }

        private void TenhoAExpressaoDaConta()
        {
            Assert.AreEqual("2 + 3", _expressao);

        }
    }

    internal class TweetParser
    {
        public string CalcularExpressao()
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    public class CalcularTweet
    {
        private TweetParaProcessar _tweetParaProcessar;
        private double _resultado;

        [Test]
        public void CalcularTweetTestes()
        {
            new Historia("calcular um tweet")
               .Para("saber quanto da um tweet")
               .Enquanto("usuario do quantoeh")
               .EuQuero("que o quanto eh calcule meus tweets")

               .ComCenario("tweet bem formado")
               .Dado(UmTweetBemFormado)
               .Quando(SolicitoUmResultadoAEsteTweet)
               .Entao(TenhoOResultaoEsperado)

               .Execute();

        }

        private void UmTweetBemFormado()
        {
            _tweetParaProcessar = new TweetParaProcessar("giovannibassi @quantoeh 2 + 3");
        }

        private void SolicitoUmResultadoAEsteTweet()
        {
            _resultado = _tweetParaProcessar.Processar();
        }

        private void TenhoOResultaoEsperado()
        {
            Assert.AreEqual(5, _resultado);
        }
    }
}
using System;
using NUnit.Framework;
using QuantoEh.Dominio;
using StoryQ.pt_BR;

namespace QuantoEh.Tests.Worker
{
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
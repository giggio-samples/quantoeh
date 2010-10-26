using System;
using System.Linq;
using QuantoEh.Dominio;
using StoryQ.pt_BR;
using NUnit.Framework;

namespace QuantoEh.Tests.Worker
{
    [TestFixture]
    public class ReconhecerTweets
    {
        private string _textoTweet;
        private TweetParaProcessar _tweetParaProcessar;
        private FilaParaProcessamentoDeTweets _filaParaProcessamentoDeTweets;

        [Test]
        public void ReconhecerTweetsTests()
        {
            new Historia("reconhecer perguntas no twitter")
                .Para("saber quais tweets tenho que processar")
                .Enquanto("usuario do quantoeh")
                .EuQuero("que o quanto é reconheça meus tweets")

                        .ComCenario("twittando uma conta de soma simples")
                            .Dado(UmTextoDeUmTweetBemFormado)
                            .Quando(CrioUmTweetParaProcessar)
                            .Entao(TenhoUmTweetParaProcessar)
                                .E(OTextoDoTweetParaProcessarÉOMesmoDoTextoOriginal)

                        .ComCenario("colocando o tweet na fila de processamento")
                            .Dado(UmTextoDeUmTweetBemFormado)
                                .E(UmaFilaParaProcessamento)
                            .Quando(CrioUmTweetParaProcessar)
                                .E(ArmazenoOTweetNaFilaDeProcessamento)
                            .Entao(AFilaPossuiUmTweetParaProcessar)
                .Execute();
        }

        private void AFilaPossuiUmTweetParaProcessar()
        {
            Assert.AreEqual(1, _filaParaProcessamentoDeTweets.Pendentes);
            Assert.AreEqual(_tweetParaProcessar, _filaParaProcessamentoDeTweets.Tweets.First());
        }

        private void UmaFilaParaProcessamento()
        {
            _filaParaProcessamentoDeTweets = new FilaParaProcessamentoDeTweets();
        }

        private void ArmazenoOTweetNaFilaDeProcessamento()
        {
            _filaParaProcessamentoDeTweets.GuardarNovo(_tweetParaProcessar);
        }

        private void UmTextoDeUmTweetBemFormado()
        {
            _textoTweet = "giovannibassi @quantoeh 2 + 3";
        }

        private void CrioUmTweetParaProcessar()
        {
            _tweetParaProcessar = new TweetParaProcessar(_textoTweet);
        }

        private void TenhoUmTweetParaProcessar()
        {
            Assert.IsNotNull(_tweetParaProcessar);
        }

        private void OTextoDoTweetParaProcessarÉOMesmoDoTextoOriginal()
        {
            Assert.AreEqual(_textoTweet, _tweetParaProcessar.Texto);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using QuantoEh.Dominio;
using QuantoEh.Worker;
using StoryQ.pt_BR;

namespace QuantoEh.Tests
{
    [TestFixture]
    public class AvaliadorDeTweetsSpec_Calcular
    {
        private AvaliadorDeTweets _avaliadorDeTweets;
        private Mock<ITweetsParaProcessar> _repositorioDeTweetsParaProcessar;
        private Mock<IRespostasParaRetuitar> _respostasParaRetuitar;
        private int _calculados;

        [Test]
        public void RodarVerificadorDeTweet()
        {
            new Historia("calcular um tweet")
               .Para("saber quanto da um tweet")
               .Enquanto("usuario do quantoeh")
               .EuQuero("que o avaliador calcule meus tweets")

               .ComCenario("tweet bem formado com soma simples")
               .Dado(UmRepositorioDeTweetsParaProcessarCom2TweetsBons)
               .E(UmaListaDeRespostas)
               .E(UmAvaliadordorDeTweets)
               .Quando(OAvaliadorCalculaOsTweets)
               .Entao(AFileDeProcessamentoDeCalculoFoiZerada)
               .E(AFilaDeRetweetTem2TweetsParaRetuitar)
               .E(OAvaliadorConfirmouQue_TuitesForamProcessados, 2)

               .ComCenario("tweets bons e maus misturados")
               .Dado(UmRepositorioDeTweetsParaProcessarCom2TweetsBonsE1RuimNoMeio)
               .E(UmaListaDeRespostas)
               .E(UmAvaliadordorDeTweets)
               .Quando(OAvaliadorCalculaOsTweets)
               .Entao(AFileDeProcessamentoDeCalculoFoiZerada)
               .E(AFilaDeRetweetTem2TweetsParaRetuitar)
               .E(OAvaliadorConfirmouQue_TuitesForamProcessados, 3)

               .Execute();

        }

        private void UmRepositorioDeTweetsParaProcessarCom2TweetsBonsE1RuimNoMeio()
        {
            _repositorioDeTweetsParaProcessar = new Mock<ITweetsParaProcessar>();
            var listaDeTweetsNovos = new List<TweetParaProcessar>
                                                          {
                                                              new TweetParaProcessar("testequantoeh", "@quantoeh 2 + 3", 2),
                                                              new TweetParaProcessar("testequantoeh", "@quantoeh 9 ** 999", 2),
                                                              new TweetParaProcessar("testequantoeh", "@quantoeh 2 * 3", 3)
                                                          };
            _repositorioDeTweetsParaProcessar.Setup(r => r.ObterTodos()).Returns(listaDeTweetsNovos);
        }

        private void UmRepositorioDeTweetsParaProcessarCom2TweetsBons()
        {
            _repositorioDeTweetsParaProcessar = new Mock<ITweetsParaProcessar>();
            var listaDeTweetsNovos = new List<TweetParaProcessar>
                                                          {
                                                              new TweetParaProcessar("testequantoeh", "@quantoeh 2 + 3", 2),
                                                              new TweetParaProcessar("testequantoeh", "@quantoeh 2 * 3", 3)
                                                          };
            _repositorioDeTweetsParaProcessar.Setup(r => r.ObterTodos()).Returns(listaDeTweetsNovos);
        }

        private void UmAvaliadordorDeTweets()
        {
            _avaliadorDeTweets = new AvaliadorDeTweets(new Mock<IMenções>().Object, _repositorioDeTweetsParaProcessar.Object, _respostasParaRetuitar.Object, new Mock<ITimeline>().Object);
        }

        private void UmaListaDeRespostas()
        {
            _respostasParaRetuitar = new Mock<IRespostasParaRetuitar>();
        }

        private void OAvaliadorCalculaOsTweets()
        {
            _calculados = _avaliadorDeTweets.CalcularTweets();
        }

        private void AFileDeProcessamentoDeCalculoFoiZerada()
        {
            _repositorioDeTweetsParaProcessar.VerifyAll();
        }

        private void AFilaDeRetweetTem2TweetsParaRetuitar()
        {
            _respostasParaRetuitar.Verify(r => r.Adicionar(It.Is<Resposta>(res => res.Texto == "5 RT @testequantoeh: @quantoeh 2 + 3")));
            _respostasParaRetuitar.Verify(r => r.Adicionar(It.Is<Resposta>(res => res.Texto == "6 RT @testequantoeh: @quantoeh 2 * 3")));
        }

        private void OAvaliadorConfirmouQue_TuitesForamProcessados(int processadosEsperados)
        {
            Assert.AreEqual(processadosEsperados, _calculados);
        }
    }
}

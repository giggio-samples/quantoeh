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
    public class AvaliadorDeTweetsSpec_Retuitar
    {
        private AvaliadorDeTweets _avaliadorDeTweets;
        private Mock<IRespostasParaRetuitar> _respostasParaRetuitar;
        private int retuitados;

        [Test]
        public void RodarVerificadorDeTweet()
        {
            new Historia("calcular um tweet")
               .Para("saber quanto da um tweet")
               .Enquanto("usuario do quantoeh")
               .EuQuero("que o avaliador retuite os tuites calculados")

               .ComCenario("tweet bem formado com soma simples")
               .Dado(UmaListaDeRespostasCom2Respostas)
               .E(UmAvaliadordorDeTweets)
               .Quando(OAvaliadorRetuitaOsCalculosJaFeitos)
               .Entao(AFileDeProcessamentoDeCalculoFoiZerada)
               .E(OAvaliadorConfirmouQue2RetuitesForamProcessados)

               .Execute();

        }

        private void UmaListaDeRespostasCom2Respostas()
        {
            var respostas = new List<Resposta>
                                {
                                    new Resposta{Texto = "5 RT testequantoeh: @quantoeh 2 + 3",IdTweetOriginal = 1}, 
                                    new Resposta{Texto = "6 RT testequantoeh: @quantoeh 2 * 3",IdTweetOriginal = 2}
                                };
            _respostasParaRetuitar = new Mock<IRespostasParaRetuitar>();
            _respostasParaRetuitar.Setup(r => r.ObterTodas()).Returns(respostas).Verifiable();
        }   

        private void UmAvaliadordorDeTweets()
        {
            var timeline = new Mock<ITimeline>();
            timeline.Setup(t => t.Postar(It.IsAny<IEnumerable<Resposta>>())).Returns(2);
            _avaliadorDeTweets = new AvaliadorDeTweets(new Mock<IMenções>().Object, new Mock<IRepositorioDeTweetsParaProcessar>().Object, _respostasParaRetuitar.Object, timeline.Object);
        }

        private void OAvaliadorRetuitaOsCalculosJaFeitos()
        {
            retuitados = _avaliadorDeTweets.Retuitar();
        }

        private void AFileDeProcessamentoDeCalculoFoiZerada()
        {
            _respostasParaRetuitar.VerifyAll();
        }

        private void OAvaliadorConfirmouQue2RetuitesForamProcessados()
        {
            Assert.AreEqual(2, retuitados);
        }
    }
}

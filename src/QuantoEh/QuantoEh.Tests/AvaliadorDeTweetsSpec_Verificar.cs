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
    public class AvaliadorDeTweetsSpec_Verificar
    {
        private Mock<IMenções> _menções;
        private AvaliadorDeTweets _avaliadorDeTweets;
        private Mock<ITweetsParaProcessar> _repositorioDeTweetsParaProcessar;
        private AvaliadorDeTweets _outroAvaliadorDeTweets;
        private Mock<TweetsNovos> _tweetsNovos;
        private const ulong UltimoId = 15;
        private const ulong IdQuandoNaoHaUmaAnterior = 0;

        [Test]
        public void RodarVerificadorDeTweet()
        {
            new Historia("calcular um tweet")
               .Para("saber quanto da um tweet")
               .Enquanto("usuario do quantoeh")
               .EuQuero("que o worker role verifique meus tweets")

               .ComCenario("tweet bem formado com soma simples")
               .Dado(UmConjuntoDeMenções)
               .E(UmRepositorioDeTweetsParaProcessar)
               .E(UmAvaliadorDeTweets)
               .Quando(OAvaliadorVerificaOsTweets)
               .Entao(AFilaDeProcessamentoTemUmTweetParaProcessar)
               
               .ComCenario("reiniciando a verificação")
               .Dado(UmConjuntoDeMenções)
               .E(UmRepositorioDeTweetsParaProcessar)
               .E(UmAvaliadorDeTweets)
               .E(OutroAvaliadorDeTweets)
               .Quando(OAvaliadorVerificaOsTweets)
               .E(OAvaliadorVerificaNovamenteOsTweets)
               .Entao(FoiPassadoZeroComoUltimoTweetNaPrimeiraChamada)
               .E(OIdDoUltimoTweetFoiPassadoNaSegundaChamada)
               
               .Execute();

        }
        
        private void UmConjuntoDeMenções()
        {
            _tweetsNovos = new Mock<TweetsNovos>();
            _menções = new Mock<IMenções>();
            _menções.Setup(m => m.ObterNovos(IdQuandoNaoHaUmaAnterior)).Returns(_tweetsNovos.Object);
            _menções.Setup(m => m.ObterNovos(UltimoId)).Returns(_tweetsNovos.Object);
        }

        private void UmRepositorioDeTweetsParaProcessar()
        {
            _repositorioDeTweetsParaProcessar = new Mock<ITweetsParaProcessar>(MockBehavior.Strict);
            _repositorioDeTweetsParaProcessar.Setup(r => r.Adicionar(_tweetsNovos.Object));
            var id = IdQuandoNaoHaUmaAnterior;
            _repositorioDeTweetsParaProcessar.Setup(r => r.ObterUltimoId()).Returns(() => id).Callback(() => id = UltimoId);
        }

        private void UmAvaliadorDeTweets()
        {
            _avaliadorDeTweets = new AvaliadorDeTweets(_menções.Object, _repositorioDeTweetsParaProcessar.Object, new Mock<IRespostasParaRetuitar>().Object, new Mock<ITimeline>().Object);
        }

        private void OAvaliadorVerificaOsTweets()
        {
            _avaliadorDeTweets.VerificarTweetsNovos();
        }

        private void AFilaDeProcessamentoTemUmTweetParaProcessar()
        {
            _repositorioDeTweetsParaProcessar.Verify(r => r.Adicionar(_tweetsNovos.Object), Times.Once());
        }

        private void OutroAvaliadorDeTweets()
        {
            _outroAvaliadorDeTweets = new AvaliadorDeTweets(_menções.Object, _repositorioDeTweetsParaProcessar.Object, new Mock<IRespostasParaRetuitar>().Object, new Mock<ITimeline>().Object);
        }

        private void OAvaliadorVerificaNovamenteOsTweets()
        {
            _outroAvaliadorDeTweets.VerificarTweetsNovos();
        }

        private void FoiPassadoZeroComoUltimoTweetNaPrimeiraChamada()
        {
            _menções.Verify(m => m.ObterNovos(IdQuandoNaoHaUmaAnterior), Times.Once());
        }

        private void OIdDoUltimoTweetFoiPassadoNaSegundaChamada()
        {
            _menções.Verify(m => m.ObterNovos(UltimoId), Times.Once());
        }


    }
}

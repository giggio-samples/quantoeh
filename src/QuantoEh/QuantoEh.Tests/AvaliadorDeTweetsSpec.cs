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
    public class AvaliadorDeTweetsSpec
    {
        private TweetParaProcessar _tweetParaProcessar;
        private Mock<IMenções> _menções;
        private AvaliadorDeTweets _avaliadorDeTweets;
        private Mock<IRepositorioDeTweetsParaProcessar> _repositorioDeTweetsParaProcessar;
        private List<TweetParaProcessar> _listaDeTweetsNovos;
        private const long IdQuandoNaoHaUmaAnterior = 0;

        [Test]
        public void RodarVerificadorDeTweet()
        {
            new Historia("calcular um tweet")
               .Para("saber quanto da um tweet")
               .Enquanto("usuario do quantoeh")
               .EuQuero("que o worker role verifique meus tweets")

               .ComCenario("tweet bem formado com soma simples")
               .Dado(UmTweetBemFormadoComTexto_, "giovannibassi @quantoeh 2 + 3")
               .E(UmConjuntoDeMenções)
               .E(UmRepositorioDeTweetsParaProcessar)
               .E(UmVerificadorDeTweets)
               .Quando(OVerificadorVerificaOsTweets)
               .Entao(AFileDeProcessamentoTemUmTweetParProcessar)
               
               .Execute();

        }

        private void UmTweetBemFormadoComTexto_(string texto)
        {
            _tweetParaProcessar = new TweetParaProcessar(texto);
        }

        private void UmConjuntoDeMenções()
        {
            _listaDeTweetsNovos = new List<TweetParaProcessar> {_tweetParaProcessar};
            var tweetsNovos = new Mock<TweetsNovos>();
            tweetsNovos.Setup(t => t.Novos).Returns(_listaDeTweetsNovos);
            _menções = new Mock<IMenções>();
            _menções.Setup(m => m.ObterNovos(IdQuandoNaoHaUmaAnterior)).Returns(tweetsNovos.Object);
        }

        private void UmRepositorioDeTweetsParaProcessar()
        {
            _repositorioDeTweetsParaProcessar = new Mock<IRepositorioDeTweetsParaProcessar>();
        }

        private void UmVerificadorDeTweets()
        {
            _avaliadorDeTweets = new AvaliadorDeTweets(_menções.Object, _repositorioDeTweetsParaProcessar.Object);
        }

        private void OVerificadorVerificaOsTweets()
        {
            _avaliadorDeTweets.VerificarTweetsNovos();
        }

        private void AFileDeProcessamentoTemUmTweetParProcessar()
        {
            _repositorioDeTweetsParaProcessar.Verify(r => r.Adicionar(_listaDeTweetsNovos), Times.Once());
        }
    }
}

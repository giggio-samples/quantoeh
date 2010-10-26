using System;
using QuantoEh.Dominio;
using QuantoEh.Worker;
using StoryQ.pt_BR;
using NUnit.Framework;
using System.Linq;

namespace QuantoEh.Tests.Worker
{

    //historia e colocar tweets na fila de trabalho
    //para poder saber o que vou fazer
    //enquanto worker role do quanto é
    //eu quero saber que tenho trabalho pra fazer

    //com cenario Um tweet mencionando
    //dado Um tweet mencionando QuantoEh
    //quando Puxo essa mensagem do Twitter
    //entao coloco ele na fila de trabalho

    //com cenario m tweet novo, e um anterior
    //dado Um tweet mencionando QuantoEh
    //e um tweet anterior
    //quando Puxo essa mensagem do Twitter informando o último tweet puxado
    //entao coloco apenas o tweet novo na fila de trabalho

    [TestFixture]
    public class ColocarTweets
    {
        private WorkerRole _worker;

        [Test]
        public void ColocarTweetsNaFilaDeTrabalho()
        {
            new Historia("colocar tweets na fila de trabalho")
                .Para("poder saber o que vou fazer")
                .Enquanto("worker role do quanto é")
                .EuQuero("saber que tenho trabalho pra fazer")

                        .ComCenario("Um tweet mencionando")
                            .Dado(UmWorkerRole)
                                .E(UmTweetMencionandoQuantoEh)
                            .Quando(PuxoEssaMensagemDoTwitter)
                            .Entao(ColocoEleNaFilaDeTrabalho)

                        .ComCenario("m tweet novo, e um anterior")
                            .Dado(UmWorkerRole)
                                .E(UmTweetMencionandoQuantoEh)
                                .E(UmTweetAnterior)
                            .Quando(PuxoEssaMensagemDoTwitterInformandoOÚltimoTweetPuxado)
                            .Entao(ColocoApenasOTweetNovoNaFilaDeTrabalho)
                .Execute();
        }

        private void UmWorkerRole()
        {
            _worker = new WorkerRole();
        }

        private void UmTweetMencionandoQuantoEh()
        {
            //IQueryable mencoes = Menções.UltimasMençõesAPartirDe(Menções.UltimoTweet);
            //Assert.IsNotNull(mencoes);
        }

        private void PuxoEssaMensagemDoTwitter()
        {
            throw new NotImplementedException();
        }

        private void ColocoEleNaFilaDeTrabalho()
        {
            throw new NotImplementedException();
        }

        private void UmTweetAnterior()
        {
            throw new NotImplementedException();
        }

        private void PuxoEssaMensagemDoTwitterInformandoOÚltimoTweetPuxado()
        {
            throw new NotImplementedException();
        }

        private void ColocoApenasOTweetNovoNaFilaDeTrabalho()
        {
            throw new NotImplementedException();
        }
    }
}

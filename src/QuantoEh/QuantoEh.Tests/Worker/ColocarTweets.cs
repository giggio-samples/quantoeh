﻿using System;
using StoryQ.pt_BR;
using NUnit.Framework;

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
        [Test]
        public void ColocarTweetsNaFilaDeTrabalho()
        {
            new Historia("colocar tweets na fila de trabalho")
                .Para("poder saber o que vou fazer")
                .Enquanto("worker role do quanto é")
                .EuQuero("saber que tenho trabalho pra fazer")

                        .ComCenario("Um tweet mencionando")
                            .Dado(UmTweetMencionandoQuantoEh)
                            .Quando(PuxoEssaMensagemDoTwitter)
                            .Entao(ColocoEleNaFilaDeTrabalho)

                        .ComCenario("m tweet novo, e um anterior")
                            .Dado(UmTweetMencionandoQuantoEh)
                                .E(UmTweetAnterior)
                            .Quando(PuxoEssaMensagemDoTwitterInformandoOÚltimoTweetPuxado)
                            .Entao(ColocoApenasOTweetNovoNaFilaDeTrabalho)
                .Execute();
        }

        private void UmTweetMencionandoQuantoEh()
        {
            throw new NotImplementedException();
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
using System;
using NUnit.Framework;
using StoryQ.pt_BR;

namespace QuantoEh.Tests.Worker
{
    [TestFixture]
    public class RetuitarAsRespostas
    {
        // historia e retuitar as respostas agendadas
        //para que o usuario receba o retorno do que postou
        //enquanto worker role
        //eu quero enviar tweets de retorno

        //com cenario fila com um item
        //dado um item na fila de retweet
        //quando a fila é processada
        //entao vai para a timeline do QuantoEh

        //com cenario fila com nenhum item
        //dado uma fila vazia
        //quando a fila é processada
        //entao a timeline nao muda
        [Test]
        public void RetuitarAsRespostasAgendadas()
        {
            new Historia("retuitar as respostas agendadas")
                .Para("que o usuario receba o retorno do que postou")
                .Enquanto("worker role")
                .EuQuero("enviar tweets de retorno")

                .ComCenario("fila com um item")
                .Dado(UmItemNaFilaDeRetweet)
                .Quando(AFilaÉProcessada)
                .Entao(VaiParaATimelineDoQuantoEh)

                .ComCenario("fila com nenhum item")
                .Dado(UmaFilaVazia)
                .Quando(AFilaÉProcessada)
                .Entao(ATimelineNaoMuda)
                .Execute();
        }

        private void UmItemNaFilaDeRetweet()
        {
            throw new NotImplementedException();
        }

        private void AFilaÉProcessada()
        {
            throw new NotImplementedException();
        }

        private void VaiParaATimelineDoQuantoEh()
        {
            throw new NotImplementedException();
        }

        private void UmaFilaVazia()
        {
            throw new NotImplementedException();
        }

        private void ATimelineNaoMuda()
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using NUnit.Framework;
using QuantoEh.Dominio;
using QuantoEh.Worker;
using StoryQ.pt_BR;

namespace QuantoEh.Tests.Worker
{
    [TestFixture]
    public class RetuitarAsRespostas
    {
        private FilaParaRetuitar _fila;
        private string _item;
        private WorkerRole _worker;
        private Timeline _timeline;
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
                .Dado(UmTimeline)
                .E(UmWorkerComUmaFilaComUmItemParaRetuitar)
                .Quando(AFilaÉProcessada)
                .Entao(VaiParaATimelineDoQuantoEh)

                .ComCenario("fila com nenhum item")
                .Dado(UmTimeline)
                .E(UmWorkerComUmaFilaVazia)
                .Quando(AFilaÉProcessada)
                .Entao(ATimelineNaoMuda)
                .Execute();
        }

        private void UmTimeline()
        {
            _timeline = new Timeline();
        }

        private void UmWorkerComUmaFilaComUmItemParaRetuitar()
        {
            _worker = new WorkerRole(_timeline);
            _fila = _worker.Fila;
            _item = "Resultado:3 RT @giovannibassi: @QuantoEh 2+1";
            _fila.Adicionar(_item);
        }

        private void AFilaÉProcessada()
        {
            _worker.ProcessarFilaDeRespostas();
        }

        private void VaiParaATimelineDoQuantoEh()
        {
            Assert.IsTrue(_timeline.Contem(_item));
        }

        private void UmWorkerComUmaFilaVazia()
        {
            _worker = new WorkerRole(_timeline);
            _fila = _worker.Fila;
        }

        private void ATimelineNaoMuda()
        {
            Assert.AreEqual(0, _timeline.TotalDeItens);
        }
    }
}
using System;
using NUnit.Framework;
using StoryQ.pt_BR;

namespace QuantoEh.Tests.Worker
{
    [TestFixture]
    public class ExecutarItens
    {

        //historia executar itens da fila de trabalho
        //para encontrar o resultado das solicitações de trabalho
        //enquanto worker role
        //eu quero transformar a mensagem em cálculo

        //com cenario um item de soma simples
        //dado um item na fila de trabalho com $texto:2+1
        //quando executamos o item
        //entao obtemos $resultado:3
        //e agendamos o retweet com $texto:resultado_3

        //com cenario um item invalido
        //dado um item na fila de trabalho com $texto:abcdef
        //quando executamos o item
        //entao obtemos um erro
        //e agendamos o retweet com $texto:resultado_invalido
        [Test]
        public void ExecutarItensDaFilaDeTrabalho()
        {
            new Historia("Executar itens da fila de trabalho")
                .Para("encontrar o resultado das solicitações de trabalho")
                .Enquanto("worker role")
                .EuQuero("transformar a mensagem em cálculo")

                .ComCenario("um item de soma simples")
                .Dado(UmItemNaFilaDeTrabalhoCom_, "giovannibassi @QuantoEh 2+1")
                .Quando(ExecutamosOItem)
                .Entao(Obtemos_, 3)
                .E(AgendamosORetweetCom_, "Resultado:3 RT @giovannibassi: @QuantoEh 2+1")

                .ComCenario("um item invalido")
                .Dado(UmItemNaFilaDeTrabalhoCom_, "giovannibassi: @QuantoEh abcdef")
                .Quando(ExecutamosOItem)
                .Entao(ObtemosUmErro)
                .E(AgendamosORetweetCom_, "Cálculo inválido RT @giovannibassi: @QuantoEh abcdef")
                .Execute();
        }

        private void UmItemNaFilaDeTrabalhoCom_(string texto)
        {
            throw new NotImplementedException();
        }

        private void ExecutamosOItem()
        {
            throw new NotImplementedException();
        }

        private void Obtemos_(int resultado)
        {
            throw new NotImplementedException();
        }

        private void AgendamosORetweetCom_(string texto)
        {
            throw new NotImplementedException();
        }

        private void ObtemosUmErro()
        {
            throw new NotImplementedException();
        }
    }
}
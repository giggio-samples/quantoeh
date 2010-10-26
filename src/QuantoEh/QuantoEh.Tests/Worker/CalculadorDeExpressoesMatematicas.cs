using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using QuantoEh.Dominio;
using StoryQ.pt_BR;

namespace QuantoEh.Tests.Worker
{
    [TestFixture]
    public class CalculadorDeExpressoesMatematicas
    {
        private CalculadorDeExpressoes _calculador;
        private string _expressao;
        private double _resultado;

        [Test]
        public void CalcularTweetTestes()
        {
            new Historia("calcular um tweet")
               .Para("saber quanto da uma expressao")
               .Enquanto("usuario do quantoeh")
               .EuQuero("calcular uma expressao")

               .ComCenario("Soma simples")
               .Dado(UmExpressao_, "2 + 3")
               .E(UmCalculadorDeExpressoes)
               .Quando(SolicitoUmResultado)
               .Entao(ReceboOResultado_, 5d)

               .Execute();

        }

        private void ReceboOResultado_(double resultadoExperado)
        {
            Assert.AreEqual(5, resultadoExperado);
        }

        private void SolicitoUmResultado()
        {
            _resultado = _calculador.Calcular(_expressao);
        }

        private void UmExpressao_(string expressao)
        {
            _expressao = expressao;
        }

        private void UmCalculadorDeExpressoes()
        {
            _calculador = new CalculadorDeExpressoes();
        }
    }

}

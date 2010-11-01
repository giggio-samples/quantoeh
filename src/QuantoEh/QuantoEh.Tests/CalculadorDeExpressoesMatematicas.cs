using NUnit.Framework;
using QuantoEh.Dominio;
using StoryQ.pt_BR;

namespace QuantoEh.Tests
{
    [TestFixture]
    public class CalculadorDeExpressoesMatematicas
    {
        private CalculadorDeExpressoes _calculador;
        private string _expressao;
        private double _resultado;
        private ErroDeCalculoException _excecao;

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

               .ComCenario("divisao simples")
               .Dado(UmExpressao_, "12 / 3")
               .E(UmCalculadorDeExpressoes)
               .Quando(SolicitoUmResultado)
               .Entao(ReceboOResultado_, 4d)
               
               .ComCenario("elevacao simples")
               .Dado(UmExpressao_, "2 ** 5")
               .E(UmCalculadorDeExpressoes)
               .Quando(SolicitoUmResultado)
               .Entao(ReceboOResultado_, 32d)
                
               .ComCenario("decimal simples")
               .Dado(UmExpressao_, "2.5 / 1.2")
               .E(UmCalculadorDeExpressoes)
               .Quando(SolicitoUmResultado)
               .Entao(ReceboOResultado_, 2.0833333333333335)
                
               .ComCenario("divisao por zero")
               .Dado(UmExpressao_, "12 / 0")
               .E(UmCalculadorDeExpressoes)
               .Quando(SolicitoUmResultado)
               .Entao(ReceboUmErro)

               .Execute();

        }

        private void ReceboUmErro()
        {
            Assert.IsNotNull(_excecao);
            Assert.AreEqual("Internal compiler error: Attempted to divide by zero..", _excecao.Message);
        }

        private void ReceboOResultado_(double resultadoExperado)
        {
            Assert.AreEqual(resultadoExperado, _resultado);
        }

        private void SolicitoUmResultado()
        {
            try
            {
                _resultado = _calculador.Calcular(_expressao);
            }
            catch (ErroDeCalculoException ex)
            {
                _excecao = ex;
            }
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

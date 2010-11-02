using System;
using NUnit.Framework;
using QuantoEh.Dominio;
using StoryQ.pt_BR;

namespace QuantoEh.Tests
{
    [TestFixture]
    public class TweetParaProcessar_spec
    {
        private string _usuario;
        private string _texto;
        private Exception _excecao;
        private TweetParaProcessar _tweet;

        [Test]
        public void CalcularTweetTestes()
        {
            new Historia("criar um tweet para processar")
               .Para("criar um tweet")
               .Enquanto("usuario do quantoeh")
               .EuQuero("que o quanto eh calcule meus tweets")

               .ComCenario("tweet grande demais")
               .Dado(UmUsuario_EUmTexto_, "giovannibassi", "Pessoal, pra quem quiser contas com decimais no @quantoeh, vc tem que passar um decimal. Pra fazer 10/3 dar 3.33, tem que fazer 10.0/3")
               .Quando(CrioUmTweetParaProcessar)
               .Entao(ReceboUmErro)
               .E(OTweetNaoFoiCriado)

               .Execute();

        }

        private void UmUsuario_EUmTexto_(string usuario, string texto)
        {
            _usuario = usuario;
            _texto = texto;
        }

        private void CrioUmTweetParaProcessar()
        {
            try
            {
                _tweet = new TweetParaProcessar(_usuario, _texto, 2);
                Assert.Fail("Nao deveria conseguir criar.");
            }
            catch (Exception ex)
            {
                _excecao = ex;
            }
        }

        private void ReceboUmErro()
        {
            Assert.IsNotNull(_excecao);
            _excecao = null;
        }

        private void OTweetNaoFoiCriado()
        {
            Assert.IsNull(_tweet);
        }
    }
}
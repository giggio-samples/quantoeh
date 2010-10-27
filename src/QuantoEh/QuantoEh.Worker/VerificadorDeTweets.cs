using QuantoEh.Dominio;

namespace QuantoEh.Worker
{
    public class VerificadorDeTweets
    {
        private readonly IMenções _menções;
        private readonly IRepositorioDeTweetsParaProcessar _repositorioDeTweetsParaProcessar;
        private long _ultimoId;

        public VerificadorDeTweets(IMenções menções, IRepositorioDeTweetsParaProcessar repositorioDeTweetsParaProcessar)
        {
            _menções = menções;
            _repositorioDeTweetsParaProcessar = repositorioDeTweetsParaProcessar;
        }

        public void VerificarTweetsNovos()
        {
            var tweetsNovos = _menções.ObterNovos(_ultimoId);
            _ultimoId = tweetsNovos.IdMaisAlto;
            _repositorioDeTweetsParaProcessar.Adicionar(tweetsNovos.Novos);
        }

    }
}
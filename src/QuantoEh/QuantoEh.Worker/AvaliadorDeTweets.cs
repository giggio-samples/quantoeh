using System;
using QuantoEh.Dominio;

namespace QuantoEh.Worker
{
    public class AvaliadorDeTweets
    {
        private readonly IMenções _menções;
        private readonly IRepositorioDeTweetsParaProcessar _repositorioDeTweetsParaProcessar;
        private ulong _ultimoId;

        public AvaliadorDeTweets(IMenções menções, IRepositorioDeTweetsParaProcessar repositorioDeTweetsParaProcessar)
        {
            _menções = menções;
            _repositorioDeTweetsParaProcessar = repositorioDeTweetsParaProcessar;
        }

        public int VerificarTweetsNovos()
        {
            var tweetsNovos = _menções.ObterNovos(_ultimoId);
            _ultimoId = tweetsNovos.IdMaisAlto;
            _repositorioDeTweetsParaProcessar.Adicionar(tweetsNovos.Novos);
            return tweetsNovos.QuantidadeDeNovos;
        }

        public int CalcularTweets()
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Linq;
using QuantoEh.Dominio;

namespace QuantoEh.Worker
{
    public class AvaliadorDeTweets
    {
        private readonly IMenções _menções;
        private readonly IRepositorioDeTweetsParaProcessar _repositorioDeTweetsParaProcessar;
        private readonly IRespostasParaRetuitar _respostasParaRetuitar;
        private ulong _ultimoId;
        private ITimeline _timeline;

        public AvaliadorDeTweets(IMenções menções, IRepositorioDeTweetsParaProcessar repositorioDeTweetsParaProcessar, IRespostasParaRetuitar respostasParaRetuitar, ITimeline timeline)
        {
            _menções = menções;
            _repositorioDeTweetsParaProcessar = repositorioDeTweetsParaProcessar;
            _respostasParaRetuitar = respostasParaRetuitar;
            _timeline = timeline;
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
            var tweetsParaProcessar = _repositorioDeTweetsParaProcessar.ObterTodos();
            int quantidade = 0;
            foreach (var tweetParaProcessar in tweetsParaProcessar)
            {
                var resultado = tweetParaProcessar.Processar();
                _respostasParaRetuitar.Adicionar(resultado);
                quantidade++;
            }
            return quantidade;
        }

        public int Retuitar()
        {
            var respostas = _respostasParaRetuitar.ObterTodas();
            _timeline.Postar(respostas);
            return respostas.Count();
        }
    }
}
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
        private readonly ITimeline _timeline;

        public AvaliadorDeTweets(IMenções menções, IRepositorioDeTweetsParaProcessar repositorioDeTweetsParaProcessar, IRespostasParaRetuitar respostasParaRetuitar, ITimeline timeline)
        {
            _menções = menções;
            _repositorioDeTweetsParaProcessar = repositorioDeTweetsParaProcessar;
            _respostasParaRetuitar = respostasParaRetuitar;
            _timeline = timeline;
        }

        public int VerificarTweetsNovos()
        {
            var ultimoId = _repositorioDeTweetsParaProcessar.ObterUltimoId();
            var tweetsNovos = _menções.ObterNovos(ultimoId);
            _repositorioDeTweetsParaProcessar.Adicionar(tweetsNovos);
            return tweetsNovos.QuantidadeDeNovos;
        }

        public int CalcularTweets()
        {
            var tweetsParaProcessar = _repositorioDeTweetsParaProcessar.ObterTodos();
            int quantidade = 0;
            foreach (var tweetParaProcessar in tweetsParaProcessar)
            {
                var resultado = tweetParaProcessar.ProcessarResposta();
                _respostasParaRetuitar.Adicionar(resultado);
                quantidade++;
            }
            return quantidade;
        }

        public int Retuitar()
        {
            var respostas = _respostasParaRetuitar.ObterTodas();
            var totalRespostas = _timeline.Postar(respostas);
            return totalRespostas;
        }
    }
}
using System;
using System.Diagnostics;
using QuantoEh.Dominio;

namespace QuantoEh.AdminWeb
{
    public class AvaliadorDeTweets
    {
        private readonly IMenções _menções;
        private readonly ITweetsParaProcessar _tweetsParaProcessar;
        private readonly IRespostasParaRetuitar _respostasParaRetuitar;
        private readonly ITimeline _timeline;

        public AvaliadorDeTweets(IMenções menções, ITweetsParaProcessar tweetsParaProcessar, IRespostasParaRetuitar respostasParaRetuitar, ITimeline timeline)
        {
            _menções = menções;
            _tweetsParaProcessar = tweetsParaProcessar;
            _respostasParaRetuitar = respostasParaRetuitar;
            _timeline = timeline;
        }

        public int VerificarTweetsNovos()
        {
            var ultimoId = _tweetsParaProcessar.ObterUltimoId();
            var tweetsNovos = _menções.ObterNovos(ultimoId);
            _tweetsParaProcessar.Adicionar(tweetsNovos);
            return tweetsNovos.QuantidadeDeNovos;
        }

        public int CalcularTweets()
        {
            var tweetsParaProcessar = _tweetsParaProcessar.ObterTodos();
            int quantidade = 0;
            foreach (var tweetParaProcessar in tweetsParaProcessar)
            {
                try
                {
                    var resultado = tweetParaProcessar.ProcessarResposta();
                    _respostasParaRetuitar.Adicionar(resultado);
                }
                catch (Exception exception)
                {
                    Trace.TraceError("Erro ao calcular:\n{0}", exception.ToString());
                }
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
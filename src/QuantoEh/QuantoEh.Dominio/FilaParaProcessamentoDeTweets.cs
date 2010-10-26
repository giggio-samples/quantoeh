using System.Collections.Generic;

namespace QuantoEh.Dominio
{
    public class FilaParaProcessamentoDeTweets
    {
        private List<TweetParaProcessar> _tweetsParaProcessar = new List<TweetParaProcessar>();

        public int Pendentes
        {
            get 
            {
                return _tweetsParaProcessar.Count;
            }
        }

        public IEnumerable<TweetParaProcessar> Tweets { get { return _tweetsParaProcessar;  } }

        public void GuardarNovo(TweetParaProcessar tweetParaProcessar)
        {
            _tweetsParaProcessar.Add(tweetParaProcessar);
        }
    }
}
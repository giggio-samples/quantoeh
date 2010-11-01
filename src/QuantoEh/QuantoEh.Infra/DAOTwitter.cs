using System;
using System.Collections.Generic;
using System.Linq;
using LinqToTwitter;
using QuantoEh.Dominio;

namespace QuantoEh.Infra
{
    public class DAOTwitter : IMenções, ITimeline
    {
        public TweetsNovos ObterNovos(ulong ultimoId)
        {
            var context = new TwitterContext();
            ultimoId = ultimoId <= 0 ? 1 : ultimoId;
            var queryResults = (from t in context.Search
                                where t.Type == SearchType.Search &&
                                      t.Query == "to:giovannibassi" &&
                                      t.Page == 1 && 
                                      t.ShowUser == true &&
                                      t.PageSize == 2  &&
                                      t.SinceID == ultimoId
                                select t).Single();
            var tweets = queryResults.Entries;
            var resultado = (from r in tweets
                            select r.Title).ToList();
            if (resultado.Count == 0)
                return TweetsNovos.Vazio(ultimoId);
            var novoUltimoId = tweets.Max(t => Convert.ToUInt64(t.ID.Split(new []{':'},StringSplitOptions.RemoveEmptyEntries)[2]));
            var tweetsNovos = new TweetsNovos(resultado, novoUltimoId);
            return tweetsNovos;
        }

        public void Postar(IEnumerable<string> respostas)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Diagnostics;
using System.Linq;

namespace QuantoEh.Infra
{
    public class AvaliadorDeInterrupcao
    {
        private const string NomeTabela = "InterromperTemporariamente";

        public bool VerificaInterrompeTemporariamente()
        {
            var ctx = ConfiguracaoArmazenamentoAzure.ObterTabela(NomeTabela);
            var interromper = ctx.CreateQuery<Interromper>(NomeTabela).FirstOrDefault();
            if (interromper != null)
            {
                return interromper.GetInterrompe();
            }
            var novoInterrompe = new Interromper();
            novoInterrompe.SetInterrompe(false);
            ctx.AddObject(NomeTabela, novoInterrompe);
            ctx.SaveChanges();
            return false;
        }

        public void SetaInterrupcao(bool valor)
        {
            var ctx = ConfiguracaoArmazenamentoAzure.ObterTabela(NomeTabela);
            var ultimo = ctx.CreateQuery<Interromper>(NomeTabela).FirstOrDefault();
            if (ultimo != null)
            {
                ctx.DeleteObject(ultimo);
                ctx.SaveChanges();
            }
            var novoInterrompe = new Interromper();
            novoInterrompe.SetInterrompe(valor);
            ctx.AddObject(NomeTabela, novoInterrompe);
            ctx.SaveChanges();
            Trace.TraceInformation("Interrompido: {0}", valor);
        }
    }
}

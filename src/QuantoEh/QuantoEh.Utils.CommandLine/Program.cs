using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantoEh.Infra;

namespace QuantoEh.Utils.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //var tweetsParaProcessar = new TweetsParaProcessar();
                //var respostasParaRetuitar = new RespostasParaRetuitar();
                //tweetsParaProcessar.ResetFila();
                //respostasParaRetuitar.ResetFila();

                ConfiguracaoArmazenamentoAzure.DroparTabela("WADLogsTable");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            Console.ReadKey();
        }
    }
}

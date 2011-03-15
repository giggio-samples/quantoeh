using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using QuantoEh.Infra;

namespace QuantoEh.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly AvaliadorDeTweets _avaliadorDeTweets;
        private bool _continuar = true;
        private Task _encontrarTweets;
        private Task _calcular;
        private Task _retuitar;

        public WorkerRole()
        {
            try
            {
                var tweetsParaProcessar = new TweetsParaProcessar();
                var respostasParaRetuitar = new RespostasParaRetuitar();
                var daoTwitter = new DAOTwitter();
                _avaliadorDeTweets = new AvaliadorDeTweets(daoTwitter, tweetsParaProcessar, respostasParaRetuitar, daoTwitter);
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro ao criar o verificador de tweets.\n", exception.ToString());
            }
        }

        public override void Run()
        {
            Trace.TraceInformation("QuantoEh.Worker iniciado");
            Trace.WriteLine("QuantoEh.Worker iniciado", "Information");

            int pesquisa, calculo, retuite;
            ObterTempos(out pesquisa, out calculo, out retuite);

            IniciarTarefaDeEncontrarTweets(pesquisa);
            IniciarTarefaDeCalculo(calculo);
            IniciarTarefaDeRetuite(retuite);
            RodarIndefinidamenteAteTarefasConcluirem();
        }

        private void ObterTempos(out int pesquisa, out int calculo, out int retuite)
        {
            pesquisa = Convert.ToInt32(ConfigurationManager.AppSettings["TempoEntrePesquisa"]);
            calculo = Convert.ToInt32(ConfigurationManager.AppSettings["TempoEntreCalculo"]);
            retuite = Convert.ToInt32(ConfigurationManager.AppSettings["TempoEntreRetuite"]);
        }

        private void RodarIndefinidamenteAteTarefasConcluirem()
        {
            try
            {
                Task.WaitAll(_encontrarTweets, _calcular, _retuitar);
            }
            catch (Exception exception)
            {
                Trace.TraceError("Um erro ocorreu: \n" + exception.Message);
                throw;
            }
        }

        private void IniciarTarefaDeRetuite(int espera)
        {
            _retuitar = new Task(() =>
            {
                while (_continuar)
                {
                    Retuitar();
                    Thread.Sleep(espera * 1000);
                }
            }, TaskCreationOptions.LongRunning);
            _retuitar.Start();
        }

        private void IniciarTarefaDeCalculo(int espera)
        {
            _calcular = new Task(() =>
                                  {
                                      while (_continuar)
                                      {
                                          Calcular();
                                          Thread.Sleep(espera * 1000);
                                      }
                                  }, TaskCreationOptions.LongRunning);
            _calcular.Start();
        }

        private void IniciarTarefaDeEncontrarTweets(int espera)
        {
            _encontrarTweets = new Task(() =>
                                            {
                                                while (_continuar)
                                                {
                                                    EncontrarTweets();
                                                    Thread.Sleep(espera * 1000);
                                                }
                                            }, TaskCreationOptions.LongRunning);
            _encontrarTweets.Start();
        }

        private void EncontrarTweets()
        {
            int quantidadeDeNovos = 0;
            try
            {
                quantidadeDeNovos = _avaliadorDeTweets.VerificarTweetsNovos();
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro:\n{0}", exception.ToString());   
            }
            Trace.TraceInformation("Encontrados {0} tweets.", quantidadeDeNovos);
        }

        private void Calcular()
        {
            int calculados = 0;
            try
            {
                calculados = _avaliadorDeTweets.CalcularTweets();
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro:\n{0}", exception.ToString());
            }
            Trace.TraceInformation("Calculados {0} tweets.", calculados);
        }

        private void Retuitar()
        {
            int retuitados = 0;
            try
            {
                retuitados = _avaliadorDeTweets.Retuitar();
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro:\n{0}", exception.ToString());
            }
            Trace.TraceInformation("Retuitados {0} tweets.", retuitados);
        }


        public override void OnStop()
        {
            _continuar = false;
            Task.WaitAll(_encontrarTweets, _calcular);
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            var config = DiagnosticMonitor.GetDefaultInitialConfiguration();
            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
            DiagnosticMonitor.Start("DiagnosticsConnectionString", config);

            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }

    }
}

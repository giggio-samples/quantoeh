using System;
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

        public WorkerRole()
        {
            try
            {
                var daoTwitter = new DAOTwitter();
                _avaliadorDeTweets = new AvaliadorDeTweets(daoTwitter, new RepositorioDeTweetsParaProcessar(), new RespostasParaRetuitar(), daoTwitter);
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro ao criar o verificador de tweets.\n", exception.ToString());
            }
        }

        public override void Run()
        {
            Trace.TraceInformation("QuantoEh.Worker iniciado");
            IniciarTarefaDeEncontrarTweets();
            IniciarTarefaDeCalculo();
            RodarIndefinidamenteAteTarefasConcluirem();
        }

        private void RodarIndefinidamenteAteTarefasConcluirem()
        {
            try
            {
                Task.WaitAll(_encontrarTweets, _calcular);
            }
            catch (Exception exception)
            {
                Trace.TraceError("Um erro ocorreu: \n" + exception.Message);
                throw;
            }
        }

        private void IniciarTarefaDeCalculo()
        {
            _calcular = new Task(() =>
                                  {
                                      while (_continuar)
                                      {
                                          Calcular();
                                          Thread.Sleep(2000);
                                      }
                                  }, TaskCreationOptions.LongRunning);
            _calcular.Start();
        }

        private void IniciarTarefaDeEncontrarTweets()
        {
            _encontrarTweets = new Task(() =>
                                            {
                                                while (_continuar)
                                                {
                                                    EncontrarTweets();
                                                    Thread.Sleep(10000);
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


        public override void OnStop()
        {
            _continuar = false;
            Task.WaitAll(_encontrarTweets, _calcular);
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            //DiagnosticMonitor.Start(ConfiguracaoArmazenamentoAzure.ObterContaDeArmazenamento(), DiagnosticMonitor.GetDefaultInitialConfiguration());
            DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

    }
}

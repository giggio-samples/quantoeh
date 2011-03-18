using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using QuantoEh.Infra;
using Microsoft.WindowsAzure.Diagnostics.Management;

namespace QuantoEh.AdminWeb
{
    public class WebRole : RoleEntryPoint
    {
        private readonly AvaliadorDeTweets _avaliadorDeTweets;
        private bool _continuar = true;
        private bool _interromperTemporariamente;
        private Task _encontrarTweets;
        private Task _calcular;
        private Task _retuitar;
        private Task _interromper;
        private readonly AvaliadorDeInterrupcao _avaliadorDeInterrupcao;

        public WebRole()
        {
            try
            {
                var tweetsParaProcessar = new TweetsParaProcessar();
                var respostasParaRetuitar = new RespostasParaRetuitar();
                var daoTwitter = new DAOTwitter();
                _avaliadorDeTweets = new AvaliadorDeTweets(daoTwitter, tweetsParaProcessar, respostasParaRetuitar, daoTwitter);
                _avaliadorDeInterrupcao = new AvaliadorDeInterrupcao();
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro ao criar o verificador de tweets.\n", exception.ToString());
            }
        }

        public override void Run()
        {
            Trace.TraceInformation("QuantoEh.AdminWeb iniciado");

            var pesquisa = Config.TempoEntrePesquisa;
            var calculo = Config.TempoEntreCalculo;
            var retuite = Config.TempoEntreRetuite;
            var interrupcaoTemporaria = Config.TempoEntreInterrupcaoTemporaria;

            IniciarTarefaDeEncontrarTweets(pesquisa);
            IniciarTarefaDeCalculo(calculo);
            IniciarTarefaDeRetuite(retuite);
            IniciarTarefaDeInterrupcaoTemporaria(interrupcaoTemporaria);
            RodarIndefinidamenteAteTarefasConcluirem();
        }

        private void RodarIndefinidamenteAteTarefasConcluirem()
        {
            try
            {
                Task.WaitAll(_encontrarTweets, _calcular, _retuitar, _interromper);
            }
            catch (Exception exception)
            {
                Trace.TraceError("Um erro ocorreu: \n" + exception.Message);
                throw;
            }
        }

        private void IniciarTarefaDeInterrupcaoTemporaria(int espera)
        {
            _interromper = new Task(() =>
            {
                while (_continuar)
                {
                    VerificarInterrupcao();
                    Thread.Sleep(espera * 1000);
                }
            }, TaskCreationOptions.LongRunning);
            _interromper.Start();
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
            if (_interromperTemporariamente) return;

            int quantidadeDeNovos = 0;
            try
            {
                quantidadeDeNovos = _avaliadorDeTweets.VerificarTweetsNovos();
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro ao encontrar twits:\n{0}", exception.ToString());   
            }
            Trace.TraceInformation("Encontrados {0} tweets.", quantidadeDeNovos);
        }

        private void Calcular()
        {
            if (_interromperTemporariamente) return;

            int calculados = 0;
            try
            {
                calculados = _avaliadorDeTweets.CalcularTweets();
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro ao calcular:\n{0}", exception.ToString());
            }
            Trace.TraceInformation("Calculados {0} tweets.", calculados);
        }

        private void Retuitar()
        {
            if (_interromperTemporariamente)
                return;

            int retuitados = 0;
            try
            {
                retuitados = _avaliadorDeTweets.Retuitar();
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro ao retuitar:\n{0}", exception.ToString());
            }
            Trace.TraceInformation("Retuitados {0} tweets.", retuitados);
        }

        private void VerificarInterrupcao()
        {
            try
            {
                _interromperTemporariamente = _avaliadorDeInterrupcao.VerificaInterrompeTemporariamente();
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro ao verificar interrupção:\n{0}", exception.ToString());
            }
        }

        public override void OnStop()
        {
            _continuar = false;
            Task.WaitAll(_encontrarTweets, _calcular);
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            IniciarDiagnostico();

            return base.OnStart();
        }

        private static void IniciarDiagnostico()
        {
            //var config = DiagnosticMonitor.GetDefaultInitialConfiguration();
            //config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(5);
            //config.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;
            //DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", config);

            var roleInstanceDiagnosticManager  = ConfiguracaoArmazenamentoAzure.ObterContaDeArmazenamento().CreateRoleInstanceDiagnosticManager(RoleEnvironment.DeploymentId, RoleEnvironment.CurrentRoleInstance.Role.Name, RoleEnvironment.CurrentRoleInstance.Id);
            var config = roleInstanceDiagnosticManager.GetCurrentConfiguration();
            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(5);
            config.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;
            roleInstanceDiagnosticManager.SetCurrentConfiguration(config);
        }

    }
}
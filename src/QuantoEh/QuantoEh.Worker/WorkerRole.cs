using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using QuantoEh.Infra;

namespace QuantoEh.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly VerificadorDeTweets _verificadorDeTweets;
        private bool _continuar = true;

        public WorkerRole()
        {
            try
            {
                _verificadorDeTweets = new VerificadorDeTweets(new DAOTwitter(), new RepositorioDeTweetsParaProcessar());
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro ao criar o verificador de tweets.\n", exception.ToString());
            }
        }
        public WorkerRole(VerificadorDeTweets verificadorDeTweets)
        {
            _verificadorDeTweets = verificadorDeTweets;
        }

        public override void Run()
        {
            Trace.TraceInformation("QuantoEh.Worker iniciado");

            while (_continuar)
            {
                EncontrarTweets();
                Thread.Sleep(10000);
            }
        }

        private void EncontrarTweets()
        {
            int quantidadeDeNovos = 0;
            try
            {
                quantidadeDeNovos = _verificadorDeTweets.VerificarTweetsNovos();
            }
            catch (Exception exception)
            {
                Trace.TraceError("Erro:\n{0}", exception.ToString());   
            }
            Trace.TraceInformation("Encontrados {0} tweets.", quantidadeDeNovos);
        }


        public override void OnStop()
        {
            _continuar = false;
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

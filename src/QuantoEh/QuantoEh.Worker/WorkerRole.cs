﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace QuantoEh.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly VerificadorDeTweets _verificadorDeTweets;
        private bool _continuar;

        public WorkerRole()
        {
            
        }
        public WorkerRole(VerificadorDeTweets verificadorDeTweets)
        {
            _verificadorDeTweets = verificadorDeTweets;
        }

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("QuantoEh.Worker entry point called", "Information");

            while (_continuar)
            {
                Thread.Sleep(10000);
                _verificadorDeTweets.VerificarTweetsNovos();
                Trace.WriteLine("Working", "Information");
            }
        }

        
        public override void OnStop()
        {
            _continuar = false;
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

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

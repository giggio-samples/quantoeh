using System;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace QuantoEh.Infra
{
    public static class ConfiguracaoArmazenamentoAzure
    {
        static ConfiguracaoArmazenamentoAzure()
        {
            ConfigurarArmazenamentoAzure();            
        }

        private static CloudStorageAccount _contaDeArmazenamento;


        public static CloudQueue ObterFila(string nomeFila)
        {
            return ObterFila(ObterContaDeArmazenamento(), nomeFila);
        }

        private static CloudQueue ObterFila(CloudStorageAccount cloudStorageAccount, string nomeFila)
        {
            var cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
            var fila = cloudQueueClient.GetQueueReference(nomeFila);
            fila.CreateIfNotExist();
            return fila;
        }

        public static CloudStorageAccount ObterContaDeArmazenamento()
        {
            var contaDeArmazenamento = _contaDeArmazenamento ?? 
                                            (_contaDeArmazenamento = CloudStorageAccount.FromConfigurationSetting("DiagnosticsConnectionString"));
            return contaDeArmazenamento;
        }

        private static void ConfigurarArmazenamentoAzure()
        {
            CloudStorageAccount
                .SetConfigurationSettingPublisher((configName, configSetter) => 
                    configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)));
        }
    }
}
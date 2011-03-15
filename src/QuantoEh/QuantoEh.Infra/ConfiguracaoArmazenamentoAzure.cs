using System;
using System.Configuration;
using System.Data.Services.Client;
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
                                            (_contaDeArmazenamento = CloudStorageAccount.FromConfigurationSetting("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"));
            return contaDeArmazenamento;
        }

        private static void ConfigurarArmazenamentoAzure()
        {
            CloudStorageAccount
                .SetConfigurationSettingPublisher((configName, configSetter) =>
                                                      {
                                                          if (RoleEnvironment.IsAvailable)
                                                            configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
                                                          else
                                                          {
                                                              configSetter(ConfigurationManager.AppSettings[configName]);
                                                          }
                                                      });
        }

        public static DataServiceContext ObterTabela(string nomeTabela)
        {
            return ObterTabela(ObterContaDeArmazenamento(), nomeTabela);
        }
        private static TableServiceContext ObterTabela(CloudStorageAccount cloudStorageAccount, string nomeTabela)
        {
            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            cloudTableClient.CreateTableIfNotExist(nomeTabela);
            var ctx = new TableServiceContext(cloudStorageAccount.TableEndpoint.ToString(), cloudTableClient.Credentials);// cloudTableClient.BaseUri.ToString()
            return ctx;
        }
    }
}
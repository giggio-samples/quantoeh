using System;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace QuantoEh.Infra
{
    public static class Config
    {
        public static int TempoEntrePesquisa { get { return Convert.ToInt32(RoleEnvironment.GetConfigurationSettingValue("TempoEntrePesquisa")); } }
        public static int TempoEntreCalculo { get { return Convert.ToInt32(RoleEnvironment.GetConfigurationSettingValue("TempoEntreCalculo")); } }
        public static int TempoEntreRetuite { get { return Convert.ToInt32(RoleEnvironment.GetConfigurationSettingValue("TempoEntreRetuite")); } }
        public static int TempoEntreInterrupcaoTemporaria { get { return Convert.ToInt32(RoleEnvironment.GetConfigurationSettingValue("TempoEntreInterrupcaoTemporaria")); } }
        public static string TwitterConsumerKey { get { return RoleEnvironment.GetConfigurationSettingValue("twitterConsumerKey"); } }
        public static string TwitterConsumerSecret { get { return RoleEnvironment.GetConfigurationSettingValue("twitterConsumerSecret"); } }
        public static string TwitterOAuthToken { get { return RoleEnvironment.GetConfigurationSettingValue("twitterOAuthToken"); } }
        public static string TwitterOAuthTokenSecret { get { return RoleEnvironment.GetConfigurationSettingValue("twitterOAuthTokenSecret"); } }
        public static string TwitterUserID { get { return RoleEnvironment.GetConfigurationSettingValue("twitterUserID"); } }
        public static string TwitterScreenName { get { return RoleEnvironment.GetConfigurationSettingValue("twitterScreenName"); } }
        public static string TwitterPassword { get { return RoleEnvironment.GetConfigurationSettingValue("twitterPassword"); } }
    }
}

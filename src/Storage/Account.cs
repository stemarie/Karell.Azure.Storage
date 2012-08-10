using System.Configuration;
using Microsoft.WindowsAzure;

namespace Karell.Azure.Storage
{
    public class Account
    {
        private readonly CloudStorageAccount _account;

        public CloudStorageAccount Instance { get { return _account; } }

        //public Account(string configurationSettingName, bool hostedService)
        //{
        //    if (hostedService)
        //    {
        //        CloudStorageAccount.SetConfigurationSettingPublisher(
        //            (configName, configSettingPublisher) =>
        //            {
        //                var connectionString = RoleEnvironment.GetConfigurationSettingValue(configName);
        //                configSettingPublisher(connectionString);
        //            }
        //        );
        //    }
        //    else
        //    {
        //        CloudStorageAccount.SetConfigurationSettingPublisher(
        //            (configName, configSettingPublisher) =>
        //            {
        //                var connectionString = ConfigurationManager.ConnectionStrings[configName].ConnectionString;
        //                configSettingPublisher(connectionString);
        //            }
        //        );
        //    }

        //    _account = CloudStorageAccount.FromConfigurationSetting(configurationSettingName);
        //}

        public Account(string connectionString)
        {
            _account = CloudStorageAccount.Parse(connectionString);
        }
    }
}

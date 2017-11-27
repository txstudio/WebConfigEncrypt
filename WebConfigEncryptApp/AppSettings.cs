using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfigEncryptApp
{
    public class AppSettings
    {

        public static string DefaultLocation
        {
            get
            {
                string _settingName = "webconfig-default-location";

                if (ConfigurationManager.AppSettings[_settingName] == null)
                    return string.Empty;

                return ConfigurationManager.AppSettings[_settingName].ToString();
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfigEncryptApp
{
    public class ConfigurationSection
    {
        /// <summary>可進行加解密設定的 Element 區塊名稱</summary>
        public static IEnumerable<string> GetSections()
        {
            return new string[] { "appSettings", "connectionStrings" };
        }
    }
}

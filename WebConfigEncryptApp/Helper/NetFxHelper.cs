using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfigEncryptApp
{
    public class NetFxHelper
    {
        private static readonly string path_netfx_64 = @"C:\Windows\Microsoft.NET\Framework64\";
        private static readonly string path_netfx = @"C:\Windows\Microsoft.NET\Framework\";
        private static readonly string path_aspnet_regiis = @"aspnet_regiis.exe";

        private static readonly string[] support_versions = new string[] { "v2", "v4" };

        /// <summary>是否支援 .NET Framework 64 位元版本</summary>
        public static bool IsNetFx64()
        {
            return Directory.Exists(path_netfx_64);
        }

        /// <summary>取得 aspnet_regiis 應用程式完整路徑</summary>
        /// <param name="netfxVersion">選擇的 .NET Framework 版本</param>
        /// <param name="is64Bit">是否為 64 位元</param>
        public static string AspNetRegIISPath(string netfxVersion, bool is64Bit)
        {
            //  {root_path}/{net_framework_version}/{application.exe}

            string _root;
            string _fullPath;

            if (is64Bit == true)
                _root = path_netfx_64;
            else
                _root = path_netfx;

            _fullPath = Path.Combine(_root, netfxVersion, path_aspnet_regiis);

            return _fullPath;
        }

        /// <summary>取得 .NET Framework 支援的版本</summary>
        public static IEnumerable<string> NetFxVersion()
        {
            string _path;
            string[] _folders;
            List<string> _items;
            string _directoryName;


            //判斷從哪個資料夾取得目前的版本
            if (IsNetFx64() == true)
                _path = path_netfx_64;
            else
                _path = path_netfx;

            _folders = Directory.GetDirectories(_path);


            _items = new List<string>();

            foreach (var _folder in _folders)
            {
                _directoryName = _folder.Replace(_path, string.Empty);

                foreach (var _version in support_versions)
                    if (_directoryName.StartsWith(_version) == true)
                        _items.Add(_directoryName);
            }

            return _items;
        }
    }
}

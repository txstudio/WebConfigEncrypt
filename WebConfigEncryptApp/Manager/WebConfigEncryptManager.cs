using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfigEncryptApp
{
    /// <summary>
    /// 取得 web.config 加解密設定項目的服務
    /// </summary>
    public class WebConfigEncryptManager
    {
        private WebConfigEncryptOption _option;
        private bool _Is64Enabled;

        public WebConfigEncryptManager()
        {
            this._Is64Enabled = NetFxHelper.IsNetFx64();
        }


        /// <summary>
        /// 取得應用程式路徑
        /// </summary>
        public string GetExePath()
        {
            return NetFxHelper.AspNetRegIISPath(this._option.NetFxVersion, this._Is64Enabled);
        }

        /// <summary>
        /// 取得執行參數清單，一個 Section 會有一個參數字串
        /// </summary>
        public IEnumerable<string> GetArgs()
        {
            List<string> _args;
            StringBuilder _builder;

            _args = new List<string>();

            foreach (var _section in this._option.Sections)
            {
                _builder = new StringBuilder();


                if (this._option.IsEncrypt == true)
                    _builder.AppendFormat("-pef ");
                else
                    _builder.Append("-pdf ");


                //設定 config 區塊
                _builder.Append("\"");
                _builder.Append(_section);
                _builder.Append("\" ");


                //設定資料夾路徑
                _builder.Append("\"");
                _builder.Append(this._option.Location);
                _builder.Append("\" ");


                //若為加密時，設定演算法
                if (this._option.IsEncrypt == true)
                    _builder.Append("-prov \"RsaProtectedConfigurationProvider\"");


                _args.Add(_builder.ToString());
            }

            return _args;
        }

        /// <summary>加解密設定項目物件</summary>
        public WebConfigEncryptOption Option
        {
            get
            {
                return this._option;
            }
            set
            {
                this._option = value;
            }
        }
    }
}

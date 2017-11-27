using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfigEncryptApp
{

    /// <summary>
    /// 依設定執行指定命令提示字元內容
    /// </summary>
    public class ProcessShellManager
    {
        private Process _process;

        private string _applicationPath;
        private string[] _args;

        public ProcessShellManager()
        {
            _process = new Process();
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.CreateNoWindow = true;
        }

        public ProcessShellManager(string applicationPath, string[] args) : this()
        {
            this._applicationPath = applicationPath;
            this._args = args;
        }

        /// <summary>依照設定執行 Command</summary>
        /// <returns>執行後的輸出文字內容</returns>
        public string Start()
        {
            StringBuilder _builder;

            _builder = new StringBuilder();

            _process.StartInfo.FileName = this.ApplicationPath;
            var _args = this.Args;

            foreach (var _arg in _args)
            {
                _process.StartInfo.Arguments = _arg;
                _process.Start();
                
                var _output = _process.StandardOutput.ReadToEnd();

                //輸出執行的 command
                _builder.AppendFormat("{0} {1}", this.ApplicationPath, _arg);
                _builder.AppendLine();
                _builder.Append(_output);
            }

            return _builder.ToString();
        }

        /// <summary>應用程式執行路徑</summary>
        public string ApplicationPath
        {
            get
            {
                return this._applicationPath;
            }
            set
            {
                this._applicationPath = value;
            }
        }

        /// <summary>參數清單</summary>
        public string[] Args
        {
            get
            {
                return this._args;
            }
            set
            {
                this._args = value;
            }
        }

    }
}

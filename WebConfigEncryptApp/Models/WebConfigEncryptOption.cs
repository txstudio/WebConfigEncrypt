using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfigEncryptApp
{
    public class WebConfigEncryptOption
    {
        private List<string> _sections;
        private string _netFxVersion;

        public WebConfigEncryptOption()
        {
            this._sections = new List<string>();
        }

        public void AddSection(string section)
        {
            if (this._sections.Contains(section) == true)
                return;

            this._sections.Add(section);
        }

        public void RemoveSection(string section)
        {
            this._sections.Remove(section);
        }

        public IEnumerable<string> Sections
        {
            get
            {
                return this._sections;
            }
            set
            {
                this._sections = value.ToList();
            }
        }
        public string NetFxVersion
        {
            get
            {
                return this._netFxVersion;
            }
            set
            {
                var _version = value;
                var _valids = NetFxHelper.NetFxVersion();

                if (_valids.Contains(_version) == true)
                {
                    this._netFxVersion = value;
                }
                else
                {
                    throw new DirectoryNotFoundException(_version);
                }
            }
        }
        
        public bool IsEncrypt { get; set; }
        public string Location { get; set; }
    }
}

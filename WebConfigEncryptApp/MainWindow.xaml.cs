using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebConfigEncryptApp
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEnumerable<string> _netFxVersions = NetFxHelper.NetFxVersion();
        private readonly IEnumerable<string> _sections = ConfigurationSection.GetSections();

        private readonly WebConfigEncryptOption _option;

        private readonly WebConfigEncryptManager _webConfigEncryptManager;
        private readonly ProcessShellManager _processShellManager;


        public MainWindow()
        {
            InitializeComponent();

            _option = new WebConfigEncryptOption();

            _webConfigEncryptManager = new WebConfigEncryptManager();
            _processShellManager = new ProcessShellManager();

            //NetFxVersionStackPanel
            InitNetFxVersionControl();
            
            //SectionStackPanel
            InitSectionControl();

            this.DirectoryTextBox.Text = AppSettings.DefaultLocation;
        }

        private void InitSectionControl()
        {
            CheckBox _checkBox;

            foreach (var _section in this._sections)
            {
                _checkBox = new CheckBox();
                _checkBox.Content = _section;
                _checkBox.Click += _SectionCheckBox_Click;

                _checkBox.IsChecked = true;

                this.SectionStackPanel.Children.Add(_checkBox);
            }

            this._option.Sections = this._sections;
        }

        private void InitNetFxVersionControl()
        {
            RadioButton _radioButton = null;

            foreach (var _netFxVersion in this._netFxVersions)
            {
                _radioButton = new RadioButton();
                _radioButton.Content = _netFxVersion;

                _radioButton.Checked += _NetFxVersionRadioButton_Checked;

                this.NetFxVersionStackPanel.Children.Add(_radioButton);
            }

            if (_radioButton != null)
                _radioButton.IsChecked = true;
        }


        /// <summary>取得 web.config 加解密設定物件</summary>
        private WebConfigEncryptOption GetOption()
        {
            _option.IsEncrypt = EncryptRadioButton.IsChecked.Value;
            _option.Location = DirectoryTextBox.Text;

            return _option;
        }


        private void _SectionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var _items = this.SectionStackPanel.Children;

            foreach (UIElement item in _items)
            {
                var _checkBox = item as CheckBox;
                var _value = _checkBox.Content.ToString();

                if (_checkBox.IsChecked == true)
                    this._option.AddSection(_value);
                else
                    this._option.RemoveSection(_value);
            }
        }
        
        private void _NetFxVersionRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var _radioButton = e.Source as RadioButton;

            this._option.NetFxVersion = _radioButton.Content.ToString();
        }

        private void ProcessButtonButton_Click(object sender, RoutedEventArgs e)
        {
            this._webConfigEncryptManager.Option = this.GetOption();

            this._processShellManager.ApplicationPath = _webConfigEncryptManager.GetExePath();
            this._processShellManager.Args = _webConfigEncryptManager.GetArgs().ToArray();

            var _message = _processShellManager.Start();

            this.AppendRichText(_message);
            this.LoadConfigContent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog;

            dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = this.DirectoryTextBox.Text;
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.DirectoryTextBox.Text = dialog.FileName;
                this.LoadConfigContent();
            }
        }


        private void AppendRichText(string message)
        {
            OutputRichTextBox.AppendText(String.Format("<{0:yyyy/MM/dd HH:mm:ss}>\n", DateTime.Now));
            OutputRichTextBox.AppendText(message);
            OutputScrollViewer.ScrollToEnd();
        }

        private void LoadConfigContent()
        {
            string _fileName;
            string _path;
            string _content;
            string _lastAccessTimeString;

            DateTime _lastAccessTime;

            _fileName = "web.config";
            _lastAccessTimeString = string.Empty;

            _path = this.DirectoryTextBox.Text;

            _path = System.IO.Path.Combine(_path, _fileName);

            if (System.IO.File.Exists(_path) == true)
            {
                _content = System.IO.File.ReadAllText(_path);
                _lastAccessTime = System.IO.File.GetLastAccessTime(_path);
                _lastAccessTimeString = String.Format("{0:yyyy/MM/dd HH:mm:ss}", _lastAccessTime);
            }
            else
            {
                _content = string.Format("{0} file not found", _fileName);
            }
            
            this.FileContentTextBox.Text = _content;

            this.FilePathTextBlock.Text = _path;
            this.LastAccessTimeTextBlock.Text = _lastAccessTimeString;
        }

    }
}

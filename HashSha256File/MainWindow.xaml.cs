using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;
using System.Threading.Tasks;

namespace HashSha256File
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public static Task<string> GetHash256Async(string filename)
        {
            var task = Task.Run(() => GetHash256(filename));
            return task;
        }

        public static string GetHash256(string filename)
        {
            string hexString;
            using (var fs = new System.IO.FileStream(path: filename, mode: System.IO.FileMode.Open))
            using (var bs = new System.IO.BufferedStream(stream: fs))
            {
                using (var sha256 = new System.Security.Cryptography.SHA256Managed())
                {
                    byte[] hash = sha256.ComputeHash(inputStream: bs);
                    hexString = BitConverter.ToString(value: hash).Replace("-", String.Empty);
                }
            }
            return hexString;
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                txtInputFile.Text = ofd.FileName;
            }
            
        }

        private async void BtnCompute_Click(object sender, RoutedEventArgs e)
        {
            string fileName = txtInputFile.Text;
            if (File.Exists(fileName))
            {
                tblPrompt.Text = "Computing ...";
                string hash = await GetHash256Async(fileName);
                txtHashResult.Text = hash;
                tblPrompt.Text = String.Empty;
            }
            else
            {
                tblPrompt.Text = "File not found !!!";
            }
        }

        private void BtnCopyHash_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(text: txtHashResult.Text);
            tblPrompt.Text = "Hash is copied";
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
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

namespace Mess
{
    public partial class MainWindow : Window
    {
        private string filePath;
        System.Collections.Specialized.StringCollection propertiesLastFile = Mess.Properties.Settings.Default.LastFile;       

        public MainWindow()
        {
            InitializeComponent();
            //Properties.Settings.Default.Reset();

            for (int i = 0; i < 5; i++)
            {
                var newMenuItem = new MenuItem();
                newMenuItem.Header = propertiesLastFile[i];
                newMenuItem.Click += new RoutedEventHandler(MenuItem_OpenRecent_Click);
                OpenRecent.Items.Add(newMenuItem);
            }
        }

        private void LoadToTextBox(string path)
        {
            var content = File.ReadAllText(path);
            tbContent.Text = content;
        }

        private void MenuItem_OpenRecent_Click(object sender, RoutedEventArgs e)
        {
            var a = e.OriginalSource.ToString();
            int indexStart = a.IndexOf("Header:") + 7;
            int indexStop = a.IndexOf("Items") - 1;
            a = a.Remove(indexStop);
            filePath = a.Substring(indexStart);
            LoadToTextBox(filePath);

            if (propertiesLastFile.Contains(filePath))
            {
                propertiesLastFile.Remove(filePath);                 
            }
            propertiesLastFile.Insert(0, filePath);
            Mess.Properties.Settings.Default.Save();            
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "TXT Files (*.txt)|*.txt";

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                filePath = dialog.FileName;
                LoadToTextBox(filePath);
                if (propertiesLastFile.Count >= 5)
                {
                    propertiesLastFile.RemoveAt(4);
                }
                if (propertiesLastFile.Contains(filePath))
                {
                    propertiesLastFile.Remove(filePath);
                }
                propertiesLastFile.Insert(0, filePath);
                Mess.Properties.Settings.Default.Save();                  
            }
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ESO little project for mess files.", "About", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(filePath, tbContent.Text);
        }

        private void MenuItem_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "TXT Files (*.txt)|*.txt";
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, tbContent.Text);
            }
        }
    }
}

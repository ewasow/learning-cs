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

namespace MessParser
{
    public class InputFile
    {
        public string inputPath;

        public InputFile(string path)
        {
            this.inputPath = path;
        }
    }

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".txt";
            dialog.Filter = "TXT Files (*.txt)|*.txt";

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                var filename = new InputFile(dialog.FileName);                
                var content = File.ReadAllText(filename.inputPath);
                tbContent.Text = content;
                MessParser.Properties.Settings.Default.LastFile = filename.inputPath;   
                MessParser.Properties.Settings.Default.Save();             
            }
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ESO little project for mess files.", "About", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {            
            //File.WriteAllText();
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

        private void MenuItem_RecentFile_Click(object sender, RoutedEventArgs e)
        {
            var lastFile = MessParser.Properties.Settings.Default.LastFile;
            if (lastFile == "(empty)")
            {
                return;
            }
            var content = File.ReadAllText(lastFile);
            tbContent.Text = content;
        }
    }
}

using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace AoE_Pinger
{
    public partial class CustomFileBrowser : Window
    {
        public string? SelectedFilePath { get; private set; }

        public CustomFileBrowser()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the initial directory
            openFileDialog.InitialDirectory = Properties.Settings.Default.BuildOrderPath;

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedFilePath = openFileDialog.FileName;
                FilePathTextBlock.Text = SelectedFilePath;
                Properties.Settings.Default.BuildOrderPath = Path.GetDirectoryName(SelectedFilePath);
                Properties.Settings.Default.Save();
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

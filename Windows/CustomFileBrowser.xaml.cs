using Microsoft.Win32;
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
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedFilePath = openFileDialog.FileName;
                FilePathTextBlock.Text = SelectedFilePath;
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

using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AVSP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Add any other initialization logic here
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                // Display the selected image
                DisplayImage(imagePath);
            }
        }

        private void DisplayImage(string imagePath)
        {
            try
            {
                // Create a BitmapImage and set its source to the selected image path
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();

                // Set the Image control's source to the BitmapImage
                SelectedImage.Source = bitmap;
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., file not found, invalid image format, etc.
                MessageBox.Show($"Error displaying image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Trigger the file open action here
            BrowseButton_Click(sender, e); // For simplicity, reuse the existing method
        }
    }
}

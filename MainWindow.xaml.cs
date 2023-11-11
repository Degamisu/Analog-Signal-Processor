using Microsoft.Win32;
using System;
using System.Windows;

namespace AVSP
{
    public partial class MainWindow : Window
    {
        // Store the selected image path
        private string selectedImagePath;

        public MainWindow()
        {
            InitializeComponent();
            // Add any other initialization logic here
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                // Store the selected image path
                selectedImagePath = openFileDialog.FileName;

                // Display the selected image
                DisplayImage(selectedImagePath);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the application
            Close();
        }

        private void OpenImageViewer_Click(object sender, RoutedEventArgs e)
        {
            // Open the image viewer window and pass the stored image path
            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                ImageViewerWindow imageViewer = new ImageViewerWindow(selectedImagePath);
                imageViewer.Show();
            }
            else
            {
                MessageBox.Show("Please open a file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenOscilloscope_Click(object sender, RoutedEventArgs e)
        {
            // Open the OscilloscopeWindow and pass the stored image path
            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                OscilloscopeWindow oscilloscopeWindow = new OscilloscopeWindow(selectedImagePath);
                oscilloscopeWindow.Show();
            }
            else
            {
                MessageBox.Show("Please open a file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayImage(string imagePath)
        {
            try
            {
                // Your image display logic goes here
                // For simplicity, you can open a new window with an image viewer
                ImageViewerWindow imageViewer = new ImageViewerWindow(imagePath);
                imageViewer.Show();
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., file not found, invalid image format, etc.
                MessageBox.Show($"Error displaying image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

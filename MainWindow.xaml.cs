using Microsoft.Win32;
using System;
using System.Windows;

namespace AVSP
{
    public partial class MainWindow : Window
    {
        private string? selectedImagePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                DisplayImage(selectedImagePath);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenImageViewer_Click(object sender, RoutedEventArgs e)
        {
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
                ImageViewerWindow imageViewer = new ImageViewerWindow(imagePath);
                imageViewer.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

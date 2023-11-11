using Microsoft.Win32;
using System;
using System.Windows;

namespace AVSP
{
    public partial class MainWindow : Window
    {
        private string? selectedImagePath;
        private OscilloscopeWindow oscilloscopeWindow; // Declare oscilloscopeWindow as a class-level variable

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainInterferenceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double newValue = e.NewValue;
            UpdateInterferenceSettings(newValue);
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
                oscilloscopeWindow = new OscilloscopeWindow(selectedImagePath);
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

        public void UpdateInterferenceSettings(double newValue)
        {
            if (oscilloscopeWindow != null)
            {
                oscilloscopeWindow.UpdateRedInterference((int)newValue);
            }
        }
    }
}

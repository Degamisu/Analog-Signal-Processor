using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AVSP
{
    public partial class MainWindow : Window
    {
        private string openedImagePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                openedImagePath = openFileDialog.FileName;

                // Display the original image
                DisplayImage(openedImagePath, OriginalImage);

                // Update interference settings and display the interfered image
                UpdateInterferenceSettings();
            }
        }

        private void OpenImageViewer_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(openedImagePath))
            {
                ImageViewerWindow imageViewer = new ImageViewerWindow(openedImagePath);
                imageViewer.Show();
            }
            else
            {
                MessageBox.Show("Open an image first before opening the image viewer.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OpenOscilloscope_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(openedImagePath))
            {
                OscilloscopeWindow oscilloscope = new OscilloscopeWindow(openedImagePath);
                oscilloscope.Show();
            }
            else
            {
                MessageBox.Show("Open an image first before opening the oscilloscope.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainInterferenceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateInterferenceSettings();
        }

        private void UpdateInterferenceSettings()
        {
            int redInterference = (int)MainRedInterferenceSlider.Value;
            int greenInterference = (int)MainGreenInterferenceSlider.Value;
            int blueInterference = (int)MainBlueInterferenceSlider.Value;

            // Update the interference settings in other windows
            ImageViewerWindow?.UpdateInterferenceSettings(redInterference, greenInterference, blueInterference);
            OscilloscopeWindow?.UpdateInterferenceSettings(redInterference, greenInterference, blueInterference);

            // Display the interfered image
            DisplayInterferedImage();
        }

        private void DisplayImage(string imagePath, System.Windows.Controls.Image imageControl)
        {
            BitmapImage bitmap = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            imageControl.Source = bitmap;
        }

        private void DisplayInterferedImage()
        {
            if (!string.IsNullOrEmpty(openedImagePath))
            {
                int redInterference = (int)MainRedInterferenceSlider.Value;
                int greenInterference = (int)MainGreenInterferenceSlider.Value;
                int blueInterference = (int)MainBlueInterferenceSlider.Value;

                // Apply interference to the image
                BitmapImage interferedBitmap = ImageProcessor.ApplyInterference(openedImagePath, redInterference, greenInterference, blueInterference);

                // Display the interfered image
                DisplayImage(interferedBitmap, InterferedImage);
            }
        }

        private void DisplayImage(BitmapImage bitmap, System.Windows.Controls.Image imageControl)
        {
            imageControl.Source = bitmap;
        }
    }
}

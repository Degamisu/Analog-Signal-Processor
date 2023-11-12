using Microsoft.Win32;
using System;
using System.IO;
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

        private void DisplayInterferedImage()
        {
            if (!string.IsNullOrEmpty(openedImagePath))
            {
                int redInterference = (int)MainRedInterferenceSlider.Value;
                int greenInterference = (int)MainGreenInterferenceSlider.Value;
                int blueInterference = (int)MainBlueInterferenceSlider.Value;

                // Apply interference to the image using ImageProcessor
                BitmapImage interferedBitmap = ImageProcessor.ApplyInterference(openedImagePath, redInterference, greenInterference, blueInterference);

                // Display the interfered image
                DisplayImage(interferedBitmap, InterferedImage);
            }
        }
                // Load the original image
                BitmapImage originalBitmap = LoadBitmap(imagePath);

                // Create a writable bitmap to modify pixels
                WriteableBitmap writableBitmap = new WriteableBitmap(originalBitmap);

                // Get pixel data
                int width = originalBitmap.PixelWidth;
                int height = originalBitmap.PixelHeight;
                int stride = width * 4; // 4 bytes per pixel (RGBA)
                byte[] pixelData = new byte[height * stride];
                originalBitmap.CopyPixels(pixelData, stride, 0);

                // Apply interference to each pixel
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = y * stride + 4 * x;

                        // Apply interference to each color channel
                        pixelData[index + 2] = ApplyChannelInterference(pixelData[index + 2], redInterference); // Red channel
                        pixelData[index + 1] = ApplyChannelInterference(pixelData[index + 1], greenInterference); // Green channel
                        pixelData[index] = ApplyChannelInterference(pixelData[index], blueInterference); // Blue channel
                    }
                }

                // Update writable bitmap with modified pixel data
                writableBitmap.WritePixels(new System.Windows.Int32Rect(0, 0, width, height), pixelData, stride, 0);

                // Convert the writable bitmap back to a BitmapImage
                BitmapImage interferedBitmap = new BitmapImage();
                using (MemoryStream stream = new MemoryStream())
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(writableBitmap));
                    encoder.Save(stream);
                    interferedBitmap.BeginInit();
                    interferedBitmap.CacheOption = BitmapCacheOption.OnLoad;
                    interferedBitmap.StreamSource = stream;
                    interferedBitmap.EndInit();
                    interferedBitmap.Freeze();
                }

                return interferedBitmap;
            }

            private static byte ApplyChannelInterference(byte originalValue, int interference)
            {
                // Apply interference by adding a random value within the specified range
                Random random = new Random();
                int interferenceValue = random.Next(-interference, interference + 1);
                int result = originalValue + interferenceValue;

                // Ensure the result is within the valid byte range (0-255)
                return (byte)Math.Max(0, Math.Min(255, result));
            }

            private static BitmapImage LoadBitmap(string imagePath)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath);
                bitmap.EndInit();
                return bitmap;
            }
        }
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

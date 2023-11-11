using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AVSP
{
    public partial class OscilloscopeWindow : Window
    {
        private int redInterference;
        private int greenInterference;
        private int blueInterference;

        public OscilloscopeWindow(string imagePath, int redInterference = 0, int greenInterference = 0, int blueInterference = 0)
        {
            InitializeComponent();

            this.redInterference = redInterference;
            this.greenInterference = greenInterference;
            this.blueInterference = blueInterference;

            BitmapImage bitmap = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            ConvertImageToAnalogSignal(bitmap);
        }

        private void ConvertImageToAnalogSignal(BitmapImage bitmap)
        {
            var canvas = new Canvas();
            canvas.Width = bitmap.PixelWidth;
            canvas.Height = bitmap.PixelHeight;

            ApplyInterferenceSettings(bitmap);
            this.Content = canvas;

            for (int x = 0; x < bitmap.PixelWidth; x++)
            {
                for (int y = 0; y < bitmap.PixelHeight; y++)
                {
                    Color pixelColor = GetPixelColor(bitmap, x, y);
                    double voltage = MapColorToVoltage(pixelColor);
                    DrawPoint(canvas, x, voltage);
                }
            }
        }

        private void ApplyInterferenceSettings(BitmapImage bitmap)
        {
            for (int x = 0; x < bitmap.PixelWidth; x++)
            {
                for (int y = 0; y < bitmap.PixelHeight; y++)
                {
                    Color originalColor = GetPixelColor(bitmap, x, y);
                    Color interferedColor = ApplyInterference(originalColor);
                    bitmap.SetPixel(x, y, interferedColor); // You might need to replace this line with an appropriate method based on your requirements
                }
            }
        }

        private Color ApplyInterference(Color originalColor)
        {
            int red = (originalColor.R + redInterference) % 256;
            int green = (originalColor.G + greenInterference) % 256;
            int blue = (originalColor.B + blueInterference) % 256;

            return Color.FromRgb((byte)red, (byte)green, (byte)blue);
        }
    }
}

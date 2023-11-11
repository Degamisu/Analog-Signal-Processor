// Inside the OscilloscopeWindow.xaml.cs

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
        public OscilloscopeWindow(string imagePath)
        {
            // No need for InitializeComponent() when not using XAML-defined UI elements

            // Load the image and convert it to an analog signal
            BitmapImage bitmap = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            ConvertImageToAnalogSignal(bitmap);
        }

        private void ConvertImageToAnalogSignal(BitmapImage bitmap)
        {
            // Create a Canvas to draw the analog signal
            var canvas = new Canvas();
            canvas.Width = bitmap.PixelWidth;
            canvas.Height = bitmap.PixelHeight;

            // Add the Canvas to the OscilloscopeWindow
            this.Content = canvas;

            // Iterate through the image pixels
            for (int x = 0; x < bitmap.PixelWidth; x++)
            {
                for (int y = 0; y < bitmap.PixelHeight; y++)
                {
                    // Get the color of the current pixel
                    Color pixelColor = GetPixelColor(bitmap, x, y);

                    // Map the color value to a voltage level
                    double voltage = MapColorToVoltage(pixelColor);

                    // Draw a point on the Canvas based on the voltage and time (x-coordinate)
                    DrawPoint(canvas, x, voltage);
                }
            }
        }

        private Color GetPixelColor(BitmapImage bitmap, int x, int y)
        {
            // Get the color of the specified pixel
            CroppedBitmap cb = new CroppedBitmap(bitmap, new Int32Rect(x, y, 1, 1));
            byte[] pixel = new byte[4];
            cb.CopyPixels(pixel, 4, 0);

            return Color.FromRgb(pixel[2], pixel[1], pixel[0]);
        }

        private double MapColorToVoltage(Color color)
        {
            // Map the color to a voltage level (you might want to adjust the scaling)
            double grayscaleValue = color.R * 0.3 + color.G * 0.59 + color.B * 0.11;
            return grayscaleValue / 255.0 * 5.0; // Assuming a 5V range
        }

        private void DrawPoint(Canvas canvas, int x, double voltage)
        {
            // Draw a point on the Canvas based on the voltage and time (x-coordinate)
            var point = new Ellipse();
            point.Fill = Brushes.Red; // You can customize the color
            point.Width = 2;
            point.Height = 2;

            // Adjust the y-coordinate based on the voltage (you might need to scale it)
            Canvas.SetTop(point, voltage * canvas.Height);
            Canvas.SetLeft(point, x);

            // Add the point to the Canvas
            canvas.Children.Add(point);
        }
    }
}

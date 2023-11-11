using System;
using System.Threading.Tasks;
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
        private BitmapImage oscilloscopeBitmap;
        private Canvas oscilloscopeCanvas = new Canvas();
        private string selectedImagePath;

        // Corrected constructor
        public OscilloscopeWindow(string imagePath, int redInterference = 0, int greenInterference = 0, int blueInterference = 0)
        {
            InitializeComponent();

            this.redInterference = redInterference;
            this.greenInterference = greenInterference;
            this.blueInterference = blueInterference;

            selectedImagePath = imagePath;
            oscilloscopeBitmap = new BitmapImage(new Uri(selectedImagePath, UriKind.RelativeOrAbsolute));

            // Display the window first
            Show();

            // Show the generating signal message with a delay
            GenerateSignalMessage();

            // Start generating the analog signal
            ConvertImageToAnalogSignal(oscilloscopeBitmap);
        }

        private async Task GenerateSignalMessage()
        {
            // Delay for a short time before showing the message
            await Task.Delay(1000); // Adjust the delay duration as needed

            // Display the generating signal message
            MessageBox.Show("Generating Signal (This may take a while)", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ConvertImageToAnalogSignal(BitmapImage bitmap)
        {
            try
            {
                int pixelWidth = bitmap.PixelWidth;
                int pixelHeight = bitmap.PixelHeight;

                oscilloscopeCanvas.Width = pixelWidth;
                oscilloscopeCanvas.Height = pixelHeight;

                ApplyInterferenceSettings(oscilloscopeBitmap);

                var polyline = new Polyline
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 2
                };

                Parallel.For(0, pixelWidth, x =>
                {
                    double averageVoltage = 0;

                    for (int y = 0; y < pixelHeight; y++)
                    {
                        try
                        {
                            Color pixelColor = GetPixelColor(oscilloscopeBitmap, x, y);
                            double voltage = MapColorToVoltage(pixelColor);
                            averageVoltage += voltage;
                        }
                        catch (Exception ex)
                        {
                            // Log or handle the exception
                            Console.WriteLine($"Exception in inner loop: {ex.Message}");
                        }
                    }

                    averageVoltage /= pixelHeight;

                    // Add the point to the polyline inside the dispatcher
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        polyline.Points.Add(new Point(x, averageVoltage));
                    });
                });

                // Add the polyline to the canvas outside the loop
                Application.Current.Dispatcher.Invoke(() =>
                {
                    oscilloscopeCanvas.Children.Add(polyline);
                });
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Exception in ConvertImageToAnalogSignal: {ex.Message}");
            }
        }


        private void ApplyInterferenceSettings(BitmapImage bitmap)
        {
            int pixelWidth = 0;
            int pixelHeight = 0;

            // Use the Dispatcher to access UI-related properties on the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                pixelWidth = bitmap.PixelWidth;
                pixelHeight = bitmap.PixelHeight;
            });

            Parallel.For(0, pixelWidth, x =>
            {
                Parallel.For(0, pixelHeight, y =>
                {
                    Color originalColor = GetPixelColor(bitmap, x, y);

                    // Use the Dispatcher to update UI elements on the UI thread
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DrawPixel(x, y, ApplyInterference(originalColor), oscilloscopeCanvas);
                    });
                });
            });
        }


        private Color ApplyInterference(Color originalColor)
        {
            int red = (originalColor.R + redInterference) % 256;
            int green = (originalColor.G + greenInterference) % 256;
            int blue = (originalColor.B + blueInterference) % 256;

            return Color.FromRgb((byte)red, (byte)green, (byte)blue);
        }

        private void DrawPixel(int x, int y, Color color, Canvas canvas)
        {
            if (canvas != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var pixel = new Rectangle
                    {
                        Width = 1,
                        Height = 1,
                        Fill = new SolidColorBrush(color)
                    };

                    Canvas.SetLeft(pixel, x);
                    Canvas.SetTop(pixel, y);
                    canvas.Children.Add(pixel);
                });
            }
            else
            {
                
            }
        }

        private double MapColorToVoltage(Color color)
        {
            return (color.R + color.G + color.B) / 3.0;
        }

        public void UpdateRedInterference(int newValue)
        {
            redInterference = newValue;
            ApplyInterferenceSettings(oscilloscopeBitmap);
        }

        public void MainInterferenceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.UpdateInterferenceSettings(e.NewValue);
        }

        private Color GetPixelColor(BitmapImage bitmap, int x, int y)
        {
            int stride = 0;
            byte[] pixelData = null;

            // Use the Dispatcher to access UI-related properties on the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                stride = (bitmap.PixelWidth * (bitmap.Format.BitsPerPixel + 7) / 8);
                pixelData = new byte[stride * bitmap.PixelHeight];
                bitmap.CopyPixels(pixelData, stride, 0);
            });

            int index = y * stride + x * bitmap.Format.BitsPerPixel / 8;
            byte blue = pixelData[index];
            byte green = pixelData[index + 1];
            byte red = pixelData[index + 2];
            byte alpha = pixelData[index + 3];

            return Color.FromArgb(alpha, red, green, blue);
        }


    }
}

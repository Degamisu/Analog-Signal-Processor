using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AVSP
{
    public partial class ImageViewerWindow : Window, INotifyPropertyChanged
    {
        private BitmapImage _imageSource;

        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public ImageViewerWindow(string imagePath = null)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(imagePath))
            {
                // Display the image if a path is provided
                DisplayImage(imagePath);
            }

            // Set the DataContext
            DataContext = this;
        }

        private void DisplayImage(string imagePath)
        {
            try
            {
                // Create a BitmapImage and set its source to the selected image path
                ImageSource = new BitmapImage(new System.Uri(imagePath, System.UriKind.RelativeOrAbsolute));
            }
            catch (System.Exception ex)
            {
                // Handle exceptions, e.g., file not found, invalid image format, etc.
                MessageBox.Show($"Error displaying image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

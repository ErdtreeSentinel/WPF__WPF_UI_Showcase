using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class RoundImage : UserControl
    {
        public RoundImage()
        {
            InitializeComponent();
            Stretch = Stretch.UniformToFill;
        }

        // DependencyProperty для основного изображения
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(RoundImage),
                new PropertyMetadata(null));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        // DependencyProperty для запасного (default) изображения
        public static readonly DependencyProperty DefaultImageSourceProperty =
            DependencyProperty.Register(nameof(DefaultImageSource), typeof(ImageSource), typeof(RoundImage),
                new PropertyMetadata(null));

        public ImageSource DefaultImageSource
        {
            get => (ImageSource)GetValue(DefaultImageSourceProperty);
            set => SetValue(DefaultImageSourceProperty, value);
        }

        // DependencyProperty для скругления углов
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(RoundImage),
                new PropertyMetadata(new CornerRadius(0)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        // DependencyProperty для режима растяжения изображения
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(RoundImage),
                new PropertyMetadata(Stretch.Uniform));

        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        // Обработчик неудачной загрузки изображения
        private void PART_Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (DefaultImageSource != null)
            {
                if (sender is Image img)
                {
                    // Отписываемся, чтобы избежать зацикливания, если и запасное изображение не загрузится
                    img.ImageFailed -= PART_Image_ImageFailed;
                    img.Source = DefaultImageSource;
                }
            }
        }
    }
}

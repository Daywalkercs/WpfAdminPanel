using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;

namespace WpfAdminPanel.Converters
{
    public class ImagePathToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagePath && !string.IsNullOrWhiteSpace(imagePath))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);

                    // OnLoad - аналог using. Освобождает файл сразу после загрузки, чтобы не файл не блокировался
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; 
                    bitmap.EndInit();
                    return bitmap;
                }
                catch
                {
                    return DependencyProperty.UnsetValue;
                }

                //if (File.Exists(imagePath))
                //{
                //    var result = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                //    return result;
                //}
            }
            return DependencyProperty.UnsetValue; ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace ShopBudget
{
    [ValueConversion(typeof(decimal), typeof(string))]
    public class AmountConverter : IValueConverter //konwerter kwoty
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal price = (decimal)value;
            return price.ToString("C", culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string price = value.ToString();
            decimal result;
            if (Decimal.TryParse(price, NumberStyles.Any, culture, out result))
                return result;
            return value;
        }
    }

    public class DateConverter : IValueConverter //konwerter daty
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Date d = (Date)value;
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] buffers = value.ToString().Split();

            if (buffers.Length == 3)
            {
                byte day;
                string month = buffers[1];
                int year;

                if (byte.TryParse(buffers[0], out day) & MainWindow.months.Contains<string>(month) & int.TryParse(buffers[2], out year))
                    return new Date(day, month, year);
            }

            return value;    
        }
    }

    public class ImagePathConverter : IValueConverter // konwerter obrazkow i ikon
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagePath;
            try
            {
                imagePath = Path.Combine(ImageDirectory, "categories/" + (string)value + ".png");
            }
            catch(Exception ex)
            {
                imagePath = Path.Combine(ImageDirectory, "categories/" + "Zorro.png");
            }
            
            return new BitmapImage(new Uri(imagePath));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }

        public string ImageDirectory
        {
            get { return imageDirectory; }
            set { imageDirectory = value; }
        }

        private string imageDirectory = Directory.GetCurrentDirectory();
    }

    public class GroupByCategory : IValueConverter //konwerter do grupowania po kategorii
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

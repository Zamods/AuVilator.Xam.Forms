using System;
using System.Globalization;
using Xamarin.Forms;


namespace AuVilator.Xam.Forms.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                return value.ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return int.Parse(value.ToString());
            }
            return value;
        }
    }
}

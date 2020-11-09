using System;
using System.Globalization;
using Xamarin.Forms;

namespace AuVilator.Xam.Forms.Converters
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var intValue = (int)value;
            string valueToReturn = "";
            try
            {
                valueToReturn = intValue.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valueToReturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = (string)value;
            var spilttedString = stringValue.Split(' ');
            int valueToReturn = 0;
            try
            {
                valueToReturn = int.Parse(spilttedString[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valueToReturn;
        }
    }
}

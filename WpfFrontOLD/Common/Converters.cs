using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace WpfFront.Common
{
    public class ConverterObj2Visibility : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EvaluateValue(value);
        }

        private object EvaluateValue(object value)
        {
            if (value == null)
                return Visibility.Collapsed;

            else if (value.GetType().Equals(typeof(bool)) && bool.Parse(value.ToString()) == false)
                return Visibility.Collapsed;

            else if (value.GetType().Equals(typeof(int)) && int.Parse(value.ToString()) == 0)
                return Visibility.Collapsed;

            else
                return Visibility.Visible;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }



    public class ConverterNegation : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           //retorna la negacion de lo que entra
          return !(bool)value; 
        } 


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
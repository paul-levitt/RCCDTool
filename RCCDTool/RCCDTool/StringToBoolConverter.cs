using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RCCDTool
{
    class StringToBoolConverter : IValueConverter
    {
        public string FalseValue { get; set; }
        public string TrueValue { get; set; }

        //ViewModel going to View
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return FalseValue;
            
            return (bool)value ? TrueValue : FalseValue;
        }

        //View going to ViewModel
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if null, returns false. Otherwise, test if it is the TrueValue or not.
            return value?.Equals(TrueValue) ?? false;
        }
    }
}

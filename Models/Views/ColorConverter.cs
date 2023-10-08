using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
namespace FinanceSummary.Models.Views
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(Transaction))
            {
                Transaction tr = (Transaction)value;
                // Replace this with your actual condition and desired colors
                return (bool)tr.JustUpdated ? Brushes.Green : Brushes.White;
            }

            return null;
            // Your condition logic here
            // Return the appropriate color based on the condition
            // Example: return Brushes.Red if the condition is met, else return Brushes.Transparent
        }
            

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
    }
}

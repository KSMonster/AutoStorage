using SimpleTrader.Domain.Core;
using SimpleTrader.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimpleTrader.WPF.Validation
{
    public class Wrapper : DependencyObject
    {
        public static readonly DependencyProperty MaxAgeProperty =
        DependencyProperty.Register("MaxAge", typeof(int),
        typeof(Wrapper), new FrameworkPropertyMetadata(int.MaxValue));

        public int MaxAge
        {
            get { return (int)GetValue(MaxAgeProperty); }
            set { SetValue(MaxAgeProperty, value); }
        }
    }

    public class QuantityValidationRule : System.Windows.Controls.ValidationRule
    {
        public SearchViewModel ViewModel { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string text = value.ToString();
            int age;
            int.TryParse(text, out age);
            if (age <= 0 || age > this.Wrapper.MaxAge)
            {
                AppCore.Instance.IsEnabled = false;
                return new ValidationResult(false, "Invalid age.");
            }

            AppCore.Instance.IsEnabled = true;
            return ValidationResult.ValidResult;
        }

        public Wrapper Wrapper { get; set; }
    }
}

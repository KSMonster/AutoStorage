using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimpleTrader.WPF.Commands
{
    public class ItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BoxItemTemplate { get; set; }
        public DataTemplate NonBoxItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
        }
    }
}

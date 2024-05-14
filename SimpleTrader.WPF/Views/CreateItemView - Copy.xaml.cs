using SimpleTrader.Domain.Models;
using SimpleTrader.EntityFramework;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleTrader.WPF.Views
{
    /// <summary>
    /// Interaction logic for PutView.xaml
    /// </summary>
    public partial class CreateItemView : UserControl
    {

        public CreateItemView()
        {
            InitializeComponent();
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(ItemCodeTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            string name = NameTextBox.Text;
            string itemCode = ItemCodeTextBox.Text;


            var newItemType = new ItemType
            {
                ItemCode = itemCode,
                Name = name
            };
                ((CreateItemViewModel)DataContext).CreateItem(newItemType);
        }
    }
}

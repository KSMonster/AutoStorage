using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.EntityFramework;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using Xceed.Wpf.Toolkit.Primitives;
using static SimpleTrader.WPF.ViewModels.SearchViewModel;

namespace SimpleTrader.WPF.Views
{
    /// <summary>
    /// Interaction logic for SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();
            Loaded += SearchView_Loaded;
        }

        private async void KeyDown(object sender, System.Windows.RoutedEventArgs e)
        {
            string searchText = NameTextBox.Text;
            SearchResultsListBox.ItemsSource = await ((SearchViewModel)DataContext).PopulateSearchResults(searchText);
        } 
        private async void KeyUp(object sender, System.Windows.RoutedEventArgs e)
        {
            string searchText = NameTextBox.Text;
            SearchResultsListBox.ItemsSource = await ((SearchViewModel)DataContext).PopulateSearchResults(searchText);
        }
        private async void DropDownClosed(object sender, EventArgs e)
        {
            string searchText = NameTextBox.Text;
            SearchResultsListBox.ItemsSource = await ((SearchViewModel)DataContext).PopulateSearchResults(searchText);
        }
        private async void SearchView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            string searchText = NameTextBox.Text;
            SearchResultsListBox.ItemsSource = await ((SearchViewModel)DataContext).PopulateSearchResults(searchText);
        }
        private async void SearchResultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchResultsListBox.SelectedItem != null)
            {
                var selectedItem = (ItemType)SearchResultsListBox.SelectedItem;
                SelectedItemNameTextBlock.Text = selectedItem.Name;
                AvailableInBoxesListBox.ItemsSource = await ((SearchViewModel)DataContext).PopulateAvailableResults(selectedItem.Name);
                SelectedItemRemainingCountTextBlock.Text = await ((SearchViewModel)DataContext).CountAvailable(selectedItem.Name);
                ItemDetailsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ItemDetailsPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
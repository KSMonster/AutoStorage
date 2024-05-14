using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SimpleTrader.WPF.Commands.searchViewModelCommands {
    public class UpdateSearchViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly IBuyStockService _stocks;
        private readonly SearchViewModel _searchViewModel;
        private IDisposable _searchSubscription;

        public UpdateSearchViewModelCommand(IBuyStockService stocks, SearchViewModel searchViewModel) {
            _stocks = stocks;
            _searchViewModel = searchViewModel;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (_searchViewModel.QuantityText == null)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (Int32.Parse(_searchViewModel.QuantityText) > Int32.Parse(_searchViewModel.SelectedItemRemainingCountText))
            {
                MessageBox.Show("The amount requested is too big.");
                return;
            }

            await _searchViewModel.TakeOutItems(_searchViewModel.SelectedItemNameText, Int32.Parse(_searchViewModel.QuantityText));

            //v sitie tiesiog neveikia...
            //_searchViewModel.SelectedItemNameText = selectedItem.Name;
            _searchViewModel.AvailableInBoxesList = await _searchViewModel.PopulateAvailableResults(_searchViewModel.SelectedItemNameText);
            _searchViewModel.SelectedItemRemainingCountText = await _searchViewModel.CountAvailable(_searchViewModel.SelectedItemNameText);
            _searchViewModel.ItemDetailsPanelVisibility = Visibility.Visible;

            //_searchViewModel.Initilization();
            _searchViewModel.OnSearchDone();
        }
    }
}

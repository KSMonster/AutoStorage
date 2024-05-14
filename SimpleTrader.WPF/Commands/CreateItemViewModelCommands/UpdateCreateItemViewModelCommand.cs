using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Reactive;
using System.Windows;
using System.Windows.Input;

namespace SimpleTrader.WPF.Commands.CreateItemViewModelCommands {
    public class UpdateCreateItemViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly IBuyStockService _stocks;
        private readonly CreateItemViewModel _createItemViewModel;
        private IDisposable _createItemSubscription;

        public UpdateCreateItemViewModelCommand(IBuyStockService stocks, CreateItemViewModel createItemViewModel) {
            _stocks = stocks;
            _createItemViewModel = createItemViewModel;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_createItemViewModel.NameText == null || _createItemViewModel.ItemCode == null)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            string name = _createItemViewModel.NameText;
            string itemCode = _createItemViewModel.ItemCode;


            var newItemType = new ItemType
            {
                ItemCode = itemCode,
                Name = name
            };
            _createItemViewModel.CreateItem(newItemType);
            _createItemViewModel.OnCreateItemDone();
        }
    }
}

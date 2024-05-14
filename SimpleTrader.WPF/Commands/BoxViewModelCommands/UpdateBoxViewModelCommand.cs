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
    public class UpdateBoxViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly IBuyStockService _stocks;
        private readonly BoxViewModel _boxViewModel;
        private IDisposable _createItemSubscription;

        public UpdateBoxViewModelCommand(IBuyStockService stocks, BoxViewModel createItemViewModel) {
            _stocks = stocks;
            _boxViewModel = createItemViewModel;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public async void Execute(object parameter)
        {
            _boxViewModel.Initilization();
        }
    }
}

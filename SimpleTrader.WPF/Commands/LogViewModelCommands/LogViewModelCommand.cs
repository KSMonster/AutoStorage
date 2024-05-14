using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Reactive;
using System.Windows.Input;

namespace SimpleTrader.WPF.Commands.CreateItemViewModelCommands
{
    public class LogViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly IBuyStockService _stocks;
        private readonly CreateBoxesViewModel _createItemViewModel;
        private IDisposable _createBoxesSubscription;

        public LogViewModelCommand(IBuyStockService stocks, CreateBoxesViewModel createItemViewModel)
        {
            _stocks = stocks;
            _createItemViewModel = createItemViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _createItemViewModel.OnCreateBoxesDone();
        }
    }
}

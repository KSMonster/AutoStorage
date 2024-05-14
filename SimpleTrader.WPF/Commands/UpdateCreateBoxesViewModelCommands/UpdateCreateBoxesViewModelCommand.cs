using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Reactive;
using System.Windows;
using System.Windows.Input;

namespace SimpleTrader.WPF.Commands.CreateItemViewModelCommands
{
    public class UpdateCreateBoxesViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly IBuyStockService _stocks;
        private readonly CreateBoxesViewModel _createBoxesViewModel;
        private IDisposable _createBoxesSubscription;

        public UpdateCreateBoxesViewModelCommand(IBuyStockService stocks, CreateBoxesViewModel createBoxesViewModel)
        {
            _stocks = stocks;
            _createBoxesViewModel = createBoxesViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_createBoxesViewModel.BoxCount == null)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            int count = Int32.Parse(_createBoxesViewModel.BoxCount);

            _createBoxesViewModel.CreateBoxes(count);
            _createBoxesViewModel.OnCreateBoxesDone();
        }
    }
}

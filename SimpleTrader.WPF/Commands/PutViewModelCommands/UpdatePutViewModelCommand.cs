using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Windows;
using System.Reactive;
using System.Windows.Input;

namespace SimpleTrader.WPF.Commands.PutViewModelCommands
{
    public class UpdatePutViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly IBuyStockService _stocks;
        private readonly PutViewModel _putViewModel;
        private IDisposable _putSubscription;

        public UpdatePutViewModelCommand(IBuyStockService stocks, PutViewModel putViewModel)
        {
            _stocks = stocks;
            _putViewModel = putViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_putViewModel.EmptyOrNull(_putViewModel.SelectedBox, _putViewModel.SelectedType, _putViewModel.CountText))
            {
                if (_putViewModel.IsItFull(_putViewModel.SelectedBox.isFull))
                {
                    int boxId = _putViewModel.SelectedBox.Id;
                    string name = _putViewModel.SelectedType.Name;
                    int count = Int32.Parse(_putViewModel.CountText);
                    int typeId = _putViewModel.SelectedType.Id;
                    bool isFull = false;
                    if (_putViewModel.IsFull == true) { isFull = true; }

                    var newItem = new Item
                    {
                        fk_BoxId = boxId,
                        Name = name,
                        Count = count,
                        fk_ItemTypeId = typeId

                    };
                    _putViewModel.CreateItem(newItem);
                    _putViewModel.UpdateBox(boxId, isFull);
                    _putViewModel.OnPutDone();
                }
            }
        }
    }
}

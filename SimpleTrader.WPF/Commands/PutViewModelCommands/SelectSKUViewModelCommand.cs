using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Windows;
using System.Reactive;
using System.Windows.Input;
using Microsoft.Win32;
using Aspose.BarCode.BarCodeRecognition;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SimpleTrader.WPF.Commands.PutViewModelCommands
{
    public class SelectSKUViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly IBuyStockService _stocks;
        private readonly PutViewModel _putViewModel;
        private IDisposable _putSubscription;

        public SelectSKUViewModelCommand(IBuyStockService stocks, PutViewModel putViewModel)
        {
            _stocks = stocks;
            _putViewModel = putViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (_putViewModel.EmptyOrNull(_putViewModel.SelectedBox, _putViewModel.CountText))
            {
                if (_putViewModel.IsItFull(_putViewModel.SelectedBox.isFull))
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Filter = "BMP Files | *.bmp";
                    fileDialog.InitialDirectory = "C:\\Users\\ElintaRobotics\\Desktop\\sku";

                    bool? success = fileDialog.ShowDialog();
                    if (success == true)
                    {
                        string path = fileDialog.FileName;
                        //string fileName = fileDialog.SafeFileName;
                        string name = null;
                        BarCodeReader reader = new BarCodeReader(path, DecodeType.AllSupportedTypes);

                        // Read all types of barcode available on the input image
                        foreach (BarCodeResult result in reader.ReadBarCodes())
                        {
                            name = result.CodeText;
                            MessageBox.Show("CodeText: " + result.CodeText);
                        }
                        IEnumerable<ItemType> suggestions = await _putViewModel.GetSuggestions(name);
                        List<ItemType> suggestion = new List<ItemType>(suggestions);
                        int boxId = _putViewModel.SelectedBox.Id;
                        int count = Int32.Parse(_putViewModel.CountText);
                        int typeId = suggestion.First().Id;
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
}

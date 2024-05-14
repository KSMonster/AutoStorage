using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.Domain.Services;
using SimpleTrader.WPF.Commands;
using SimpleTrader.WPF.State.Navigators;
using System.Windows.Input;
using SimpleTrader.Domain.Models;
using SimpleTrader.FinancialModelingPrepAPI.Services;
using System.Collections.Generic;
using System;
using System.Reactive.Subjects;
using System.Reactive;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimpleTrader.WPF.Commands.PutViewModelCommands;
using SimpleTrader.EntityFramework.Services;
using System.Windows;
using SimpleTrader.EntityFramework;
using SimpleTrader.WPF.Commands.CreateItemViewModelCommands;

namespace SimpleTrader.WPF.ViewModels
{
    public class CreateBoxesViewModel : ViewModelBase
    {

        private readonly IBuyStockService _stocks;
        private IEnumerable<Box> _boxlist;
        private IEnumerable<Item> _itemlist;
        private IEnumerable<ItemType> _itemtypelist;
        private string _boxCount;
        private readonly ItemTypeDataService _itemTypeDataService;
        private readonly BoxDataService _boxDataService;
        private readonly LogDataService _logDataService;
        public ICommand CreateBoxesCommand { get; }

        public string BoxCount
        {
            get { return _boxCount; }
            set
            {
                _boxCount = value;
                OnPropertyChanged(nameof(BoxCount));
            }
        }
        public IEnumerable<Box> BoxList
        {
            get { return _boxlist; }
            set
            {
                _boxlist = value;
                OnPropertyChanged(nameof(BoxList));
            }
        }
        public IEnumerable<Item> ItemList
        {
            get { return _itemlist; }
            set
            {
                _itemlist = value;
                OnPropertyChanged(nameof(ItemList));
            }
        }
        public IEnumerable<ItemType> ItemTypeList
        {
            get { return _itemtypelist; }
            set
            {
                _itemtypelist = value;
                OnPropertyChanged(nameof(ItemTypeList));
            }
        }

        private readonly ReplaySubject<Unit> _createBoxesDone = new ReplaySubject<Unit>(1);

        public IObservable<Unit> CreateBoxesDone => _createBoxesDone;

        public void OnCreateBoxesDone()
        {
            _createBoxesDone.OnNext(Unit.Default);
        }

        public CreateBoxesViewModel(IBuyStockService stocks)
        {
            var dbContextFactory = new SimpleTraderDbContextFactory();
            _itemTypeDataService = new ItemTypeDataService(dbContextFactory);
            _boxDataService = new BoxDataService(dbContextFactory);
            _logDataService = new LogDataService(dbContextFactory);
            CreateBoxesCommand = new UpdateCreateBoxesViewModelCommand(_stocks, this);
            _stocks = stocks;
            Initilization();
            OnCreateBoxesDone();
        }
        public static CreateBoxesViewModel LoadCreateItem(IBuyStockService stocks)
        {
            CreateBoxesViewModel majorIndexViewModel = new CreateBoxesViewModel(stocks);
            stocks.BuyStock();
            stocks.FindItems();
            stocks.FindTypes();
            return majorIndexViewModel;
        }
        public async void Initilization()
        {
            BoxList = await _stocks.BuyStock();//
            ItemList = await _stocks.FindItems();
            ItemTypeList = await _stocks.FindTypes();

        }
        public async void CreateBoxes(int count)
        {

            try
            {
                await _boxDataService.CreateNew(count);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating Boxes: {ex.Message}");
                MessageBox.Show($"Inner exception: {ex.InnerException?.Message}");
            }
            var newLog = new Log
            {
                Operation = count + " new boxes have been created.",
                Date = DateTime.Now
            };
            await _logDataService.Create(newLog);
            MessageBox.Show("Boxes created successfully!");
        }
    }
}

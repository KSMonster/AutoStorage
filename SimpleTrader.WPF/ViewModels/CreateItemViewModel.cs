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

namespace SimpleTrader.WPF.ViewModels {
    public class CreateItemViewModel : ViewModelBase {

        private readonly IBuyStockService _stocks;
        private IEnumerable<Box> _boxlist;
        private IEnumerable<Item> _itemlist;
        private IEnumerable<ItemType> _itemtypelist;
        private string _nameText;
        private string _itemCode;
        private readonly ItemTypeDataService _itemTypeDataService;
        private readonly BoxDataService _boxDataService;
        private readonly LogDataService _logDataService;
        public ICommand CreateItemCommand { get; }

        public string NameText
        {
            get { return _nameText; }
            set
            {
                _nameText = value;
                OnPropertyChanged(nameof(NameText));
            }
        }
        public string ItemCode
        {
            get { return _itemCode; }
            set
            {
                _itemCode = value;
                OnPropertyChanged(nameof(ItemCode));
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

        private readonly ReplaySubject<Unit> _createItemDone = new ReplaySubject<Unit>(1);

        public IObservable<Unit> CreateItemDone => _createItemDone;

        public void OnCreateItemDone()
        {
            _createItemDone.OnNext(Unit.Default);
        }

        public CreateItemViewModel(IBuyStockService stocks)
        {
            var dbContextFactory = new SimpleTraderDbContextFactory();
            _itemTypeDataService = new ItemTypeDataService(dbContextFactory);
            _boxDataService = new BoxDataService(dbContextFactory);
            _logDataService = new LogDataService(dbContextFactory);
            CreateItemCommand = new UpdateCreateItemViewModelCommand(_stocks, this);
            _stocks = stocks;
            Initilization();
            OnCreateItemDone();
        }
        public static CreateItemViewModel LoadCreateItem(IBuyStockService stocks)
        {
            CreateItemViewModel majorIndexViewModel = new CreateItemViewModel(stocks);
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
        public async void CreateItem(ItemType newItem)
        {
            try
            {
                await _itemTypeDataService.Create(newItem);
                var newLog = new Log
                {
                    Operation = "ItemType " + newItem.Name.ToString() + " has been created.",
                    Date = DateTime.Now
                };
                await _logDataService.Create(newLog);
                MessageBox.Show("Item created successfully!");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating item: {ex.Message}");
                MessageBox.Show($"Inner exception: {ex.InnerException?.Message}");
            }
        }
    }
}

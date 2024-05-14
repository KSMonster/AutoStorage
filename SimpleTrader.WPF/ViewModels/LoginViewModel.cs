﻿using SimpleTrader.Domain.Services.TransactionServices;
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
using SimpleTrader.WPF.Commands.LoginViewModelCommands;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace SimpleTrader.WPF.ViewModels {
    public class LoginViewModel : ViewModelBase {

        private readonly IBuyStockService _stocks;
        private IEnumerable<Box> _boxlist;
        private IEnumerable<User> _userlist;
        private IEnumerable<Item> _itemlist;
        private IEnumerable<ItemType> _itemtypelist;
        private readonly ItemTypeDataService _itemTypeDataService;
        private readonly BoxDataService _boxDataService;
        private readonly UserDataService _userDataService;

        public IEnumerable<Box> BoxList
        {
            get { return _boxlist; }
            set
            {
                _boxlist = value;
                OnPropertyChanged(nameof(BoxList));
            }
        }
        public IEnumerable<User> UserList
        {
            get { return _userlist; }
            set
            {
                _userlist = value;
                OnPropertyChanged(nameof(UserList));
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

        public IObservable<Unit> PutDone => _createItemDone;

        public void OnCreateItemDone()
        {
            _createItemDone.OnNext(Unit.Default);
        }

        public LoginViewModel(IBuyStockService stocks)
        {
            var dbContextFactory = new SimpleTraderDbContextFactory();
            _itemTypeDataService = new ItemTypeDataService(dbContextFactory);
            _boxDataService = new BoxDataService(dbContextFactory);
            _userDataService = new UserDataService(dbContextFactory);
            _stocks = stocks;
            Initilization();
            OnCreateItemDone();
        }
        public static LoginViewModel LoginViewMode(IBuyStockService stocks)
        {
            LoginViewModel majorIndexViewModel = new LoginViewModel(stocks);
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

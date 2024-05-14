using Caliburn.Micro;
using Microsoft.Xaml.Behaviors.Core;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.Domain.Services.TransactionServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Reactive;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.EntityFramework;
using SimpleTrader.WPF.Commands.PutViewModelCommands;

namespace SimpleTrader.WPF.ViewModels
{
    public class BoxViewModel : ViewModelBase
    {

        private readonly IBuyStockService _stocks;
        private IEnumerable<Box> _boxlist;
        private ObservableCollection<Item> _itemlist;

        public ICollectionView ItemsView { get; set; }

        private Box _selectedBox;
        public Box SelectedBox { get=>_selectedBox; set
            {
                _selectedBox = value;
                ItemsView?.Refresh();
                OnPropertyChanged(nameof(SelectedBox));
            }
        }

        public IEnumerable<Box> BoxList
        {
            get { return _boxlist; }
            set { _boxlist = value;
                OnPropertyChanged(nameof(BoxList));
            }
        }
        public ObservableCollection<Item> ItemList
        {
            get { return _itemlist; }

            set{ _itemlist = value;
                OnPropertyChanged(nameof(ItemList));
            }
        }
        public BoxViewModel(IBuyStockService stocks)
        {
            _stocks = stocks;
            Initilization();
            OnBoxDone();
        }

        private readonly ReplaySubject<Unit> _boxDone = new ReplaySubject<Unit>(1);

        public IObservable<Unit> BoxDone => _boxDone;

        public void OnBoxDone()
        {
            _boxDone.OnNext(Unit.Default);
        }
        public static BoxViewModel LoadMajorIndexViewModel(IBuyStockService stocks)
        {
            BoxViewModel majorIndexViewModel = new BoxViewModel(stocks);
            stocks.BuyStock();
            stocks.FindItems();
            return majorIndexViewModel;
        }
        public async void Initilization()
        {
            BoxList = await _stocks.BuyStock();
            ItemList = new ObservableCollection<Item>( await _stocks.FindItems());
            ItemsView = System.Windows.Data.CollectionViewSource.GetDefaultView(ItemList);
            ItemsView.Filter = o =>
            {
                Item i = o as Item;
                if(i.Box == null)return false;
                if(SelectedBox == null)return false;
                return i.Box?.Id == SelectedBox.Id;
            };
            ItemList.CollectionChanged += (s, e) => { ItemsView.Refresh(); };
            ItemsView.Refresh();

        }

    }
}

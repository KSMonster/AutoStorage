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
using System.Threading.Tasks;

namespace SimpleTrader.WPF.ViewModels {
    public class PutViewModel : ViewModelBase {

        private readonly IBuyStockService _stocks;
        private IEnumerable<Box> _boxlist;
        private Box _selectedBox;
        private ItemType _selectedType;
        private string _count;
        private bool _isFull;
        private IEnumerable<Item> _itemlist;
        private IEnumerable<ItemType> _itemtypelist;
        private readonly ExtraItemDataService _itemDataService;
        private readonly LogDataService _logDataService;
        private readonly ItemTypeDataService _itemTypeDataService;
        private readonly BoxDataService _boxDataService;
        public ICommand PutItemCommand { get; }
        public ICommand SelectSKUCommand { get; }

        public bool IsFull
        {
            get
            {
                return _isFull;
            }
            set
            {
                _isFull = value;
                OnPropertyChanged(nameof(IsFull));
            }
        }
        public string CountText
        {
            get { return _count; }
            set 
            { 
                _count = value;
                OnPropertyChanged(nameof(CountText));
            }
        }

        #region boxlist
        public IEnumerable<Box> BoxList
        {
            get { return _boxlist; }
            set
            {
                _boxlist = value;
                OnPropertyChanged(nameof(BoxList));
            }
        }
        public Box SelectedBox
        {
            get
            {
                return _selectedBox;
            }
            set
            {
                _selectedBox = value;
                OnPropertyChanged(nameof(SelectedBox));
            }
        }
        #endregion

        #region itemlist
        public IEnumerable<Item> ItemList
        {
            get { return _itemlist; }
            set
            {
                _itemlist = value;
                OnPropertyChanged(nameof(ItemList));
            }
        }
        #endregion

        #region itemtypelist
        public IEnumerable<ItemType> ItemTypeList
        {
            get { return _itemtypelist; }
            set
            {
                _itemtypelist = value;
                OnPropertyChanged(nameof(ItemTypeList));
            }
        }
        public ItemType SelectedType
        {
            get
            {
                return _selectedType;
            }
            set
            {
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }
        #endregion

        #region putdone???
        private readonly ReplaySubject<Unit> _putDone = new ReplaySubject<Unit>(1);

        public IObservable<Unit> PutDone => _putDone;

        public void OnPutDone()
        {
            _putDone.OnNext(Unit.Default);
        }
        #endregion
        
        public PutViewModel(IBuyStockService stocks)
        {
            var dbContextFactory = new SimpleTraderDbContextFactory();
            _itemDataService = new ExtraItemDataService(dbContextFactory);
            _itemTypeDataService = new ItemTypeDataService(dbContextFactory);
            _boxDataService = new BoxDataService(dbContextFactory);
            _logDataService = new LogDataService(dbContextFactory);
            PutItemCommand = new UpdatePutViewModelCommand(_stocks, this);
            SelectSKUCommand = new SelectSKUViewModelCommand(_stocks, this);
            _stocks = stocks;
            Initilization();
            OnPutDone();
        }

        #region 0 refs???
        public static PutViewModel LoadPut(IBuyStockService stocks)
        {
            PutViewModel majorIndexViewModel = new PutViewModel(stocks);
            stocks.BuyStock();
            stocks.FindItems();
            stocks.FindTypes();
            return majorIndexViewModel;
        }
        #endregion

        public async void Initilization()
        {
            BoxList = await _stocks.BuyStock();
            ItemList = await _stocks.FindItems();
            ItemTypeList = await _stocks.FindTypes();
        }

        #region action things
        public bool EmptyOrNull(Box selectedBox, ItemType selectedType, string countText)
        {
            if (selectedBox == null || selectedType == null || string.IsNullOrEmpty(countText))
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }
            return true;
        }
        public bool IsItFull(bool isFull)
        {
            if (isFull)
            {
                MessageBox.Show("The box is full.");
                return false;
            }
            return true;
        }
        public async void CreateItem(Item newItem)
        {
            try
            {
                Item id = null;
                IEnumerable<Item> suggestions = await _itemDataService.GetSuggestions(newItem.Name);
                List<Item> suggestion = new List<Item>(suggestions);
                var boxes = await _boxDataService.GetBoxesForItems(suggestions);
                List<Box> box = new List<Box>(boxes);
                bool samebox = false;
                foreach (var item in suggestion)
                {
                    if (item.Name == newItem.Name)
                    {
                        foreach (var boxItem in box)
                        {
                            if (newItem.fk_BoxId == boxItem.Id)
                            {
                                samebox = true;
                            }

                        }
                        if(samebox)
                        {
                            item.Count += newItem.Count;
                            id = item;
                        }
                        
                    }
                }
                if (id==null&&samebox==false)
                {
                    await _itemDataService.Create(newItem);
                    var newLog = new Log
                    {
                        Operation = "Item " + newItem.Id.ToString() + " has been added to box " + newItem.fk_BoxId.ToString(),
                        Date = DateTime.Now
                    };
                    await _logDataService.Create(newLog);
                }
                else
                {
                    await _itemDataService.Update(id.Id,id);
                    var newLog = new Log
                    {
                        Operation = "Item " + id.Id.ToString() + " from box " + id.fk_BoxId.ToString() + " count has been updated",
                        Date = DateTime.Now
                    };
                    await _logDataService.Create(newLog);
                }

                MessageBox.Show("Item created successfully!");
                OnPutDone();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating item: {ex.Message}");
                MessageBox.Show($"Inner exception: {ex.InnerException?.Message}");
            }
        }
        public async void UpdateBox(int id, bool isfull)
        {
            try
            {
                Box updated = await _boxDataService.Get(id);
                updated.isFull = isfull;
                await _boxDataService.Update(id, updated);
                var newLog2 = new Log
                {
                    Operation = "Box " + id.ToString() + " IsFull status has been updated",
                    Date = DateTime.Now
                };
                await _logDataService.Create(newLog2);
                //MessageBox.Show("Box updated!");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating item: {ex.Message}");
                MessageBox.Show($"Inner exception: {ex.InnerException?.Message}");
            }
        }

        internal bool EmptyOrNull(Box selectedBox, string countText)
        {
            if (selectedBox == null || string.IsNullOrEmpty(countText))
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }
            return true;
        }

        internal async Task<IEnumerable<ItemType>> GetSuggestions(string name)
        {
            IEnumerable<ItemType> suggestions = await _itemTypeDataService.GetSuggestions(name);
            return suggestions;
        }
        #endregion
    }
}

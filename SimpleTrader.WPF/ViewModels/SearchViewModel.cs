using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.EntityFramework;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.WPF.Commands;
using SimpleTrader.WPF.Commands.PutViewModelCommands;
using SimpleTrader.WPF.Commands.searchViewModelCommands;
using SimpleTrader.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static SimpleTrader.WPF.ViewModels.SearchViewModel;

namespace SimpleTrader.WPF.ViewModels
{
    public class SearchViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private readonly IBuyStockService _stocks;
        private IEnumerable<Item> _itemlist;
        private IEnumerable<ItemType> _itemtypelist;
        private IEnumerable<Box> _boxlist;
        private ItemType _selectedType;
        private string _quantityText;
        private string _selectedItemRemainingCountText;
        private string _selectedItemNameText;
        private Visibility _itemDetailsPanelVisibility;
        private List<ItemWithBoxNumber> _availableInBoxesList;
        private readonly ExtraItemDataService _itemDataService;
        private readonly LogDataService _logDataService;
        private readonly ItemTypeDataService _itemTypeDataService;
        private readonly BoxDataService _boxDataService;
        public ICommand SearchItemCommand { get; }

        private bool _isButtonEnabled = false;

        public bool IsButtonEnabled
        {
            get { return _isButtonEnabled; }
            set
            {
                if (_isButtonEnabled != value)
                {
                    _isButtonEnabled = value;
                    OnPropertyChanged(nameof(IsButtonEnabled));
                }
            }
        }

        public List<ItemWithBoxNumber> AvailableInBoxesList
        {
            get { return _availableInBoxesList; }
            set
            {
                _availableInBoxesList = value;
                OnPropertyChanged(nameof(AvailableInBoxesList));
            }
        }
        public Visibility ItemDetailsPanelVisibility
        {
            get { return _itemDetailsPanelVisibility; }
            set
            {
                _itemDetailsPanelVisibility = value;
                OnPropertyChanged(nameof(ItemDetailsPanelVisibility));
            }
        }
        public string SelectedItemNameText
        {
            get { return _selectedItemNameText; }
            set
            {
                _selectedItemNameText = value;
                OnPropertyChanged(nameof(SelectedItemNameText));
            }
        }
        public string SelectedItemRemainingCountText
        {
            get { return _selectedItemRemainingCountText; }
            set
            {
                _selectedItemRemainingCountText = value;
                OnPropertyChanged(nameof(SelectedItemRemainingCountText));
            }
        }

        public string QuantityText
        {
            get { return _quantityText; }
            set
            {
                _quantityText = value;
                OnPropertyChanged(nameof(QuantityText));
            }
        }

        public class ItemWithBoxNumber
        {
            public Item Item { get; set; }
            public Box Box { get; set; }
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
        public IEnumerable<Box> BoxList
        {
            get { return _boxlist; }
            set
            {
                _boxlist = value;
                OnPropertyChanged(nameof(BoxList));
            }
        }

        private readonly ReplaySubject<Unit> _searchDone = new ReplaySubject<Unit>(1);

        public IObservable<Unit> SearchDone => _searchDone;
        public void OnSearchDone()
        {
            _searchDone.OnNext(Unit.Default);
        }

        public SearchViewModel(IBuyStockService stocks)
        {
            var dbContextFactory = new SimpleTraderDbContextFactory();
            _itemDataService = new ExtraItemDataService(dbContextFactory);
            _logDataService = new LogDataService(dbContextFactory);
            _itemTypeDataService = new ItemTypeDataService(dbContextFactory);
            _boxDataService = new BoxDataService(dbContextFactory);
            SearchItemCommand = new UpdateSearchViewModelCommand(_stocks, this);
            _stocks = stocks;
            ItemDetailsPanelVisibility = Visibility.Collapsed;
            Initilization();
            OnSearchDone();
        }
        public async Task<List<ItemWithBoxNumber>> PopulateAvailableResults(string searchText)
        {
            try
            {
                IEnumerable<Item> suggestions = await _itemDataService.GetEquals(searchText);

                var boxes = await _boxDataService.GetBoxesForItems(suggestions);

                List<ItemWithBoxNumber> itemsWithBoxNumber = new List<ItemWithBoxNumber>();

                foreach (var item in suggestions)
                {
                    var box = boxes.FirstOrDefault(b => b.Id == item.fk_BoxId);
                    if (box != null)
                    {
                        itemsWithBoxNumber.Add(new ItemWithBoxNumber { Item = item, Box = box });
                    }
                }

                return itemsWithBoxNumber;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> CountAvailable(string searchText)
        {
            try
            {
                IEnumerable<Item> suggestions = await _itemDataService.GetEquals(searchText);

                var boxes = await _boxDataService.GetBoxesForItems(suggestions);

                List<ItemWithBoxNumber> itemsWithBoxNumber = new List<ItemWithBoxNumber>();
                int count = 0;

                foreach (var item in suggestions)
                {
                    count = count + item.Count;
                }
                SelectedItemNameText = searchText;
                SelectedItemRemainingCountText = count.ToString();
                return count.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<ItemWithBoxNumber>> TakeOutItems(string itemname, int count)
        {
            try
            {
                IEnumerable<Item> suggestions = await _itemDataService.GetEquals(itemname);

                List<Item> suggestion = new List<Item>(suggestions);
                suggestion  = suggestion.OrderByDescending(s => s.Count).ToList();

                var boxes = await _boxDataService.GetBoxesForItems(suggestions);

                List<ItemWithBoxNumber> itemsWithBoxes = new List<ItemWithBoxNumber>();
                int save = 0;
                int countsave = int.MaxValue;
                int extra = 0;

                while (count > 0)
                {
                    countsave = int.MaxValue;
                    foreach (var item in suggestion)
                    {
                        if (item.Count < countsave)
                        {
                            countsave = item.Count;
                            save = item.fk_BoxId;
                        }
                    }
                    foreach (var item in suggestion)
                    {
                        if (item.fk_BoxId == save)
                        {
                            extra = count;
                            count = count - item.Count;
                            if (count >= 0)
                            {

                                await _itemDataService.Delete(item.Id);
                                var newLog = new Log
                                {
                                    Operation = "Item "+item.Id.ToString()+" has been deleted from box "+ item.fk_BoxId.ToString(),
                                    Date = DateTime.Now
                                };
                                await _logDataService.Create(newLog);
                                var box = boxes.FirstOrDefault(b => b.Id == item.fk_BoxId);
                                box.isFull = false;
                                await _boxDataService.Update(item.fk_BoxId, box);
                                var newLog2 = new Log
                                {
                                    Operation = "Box " + item.fk_BoxId.ToString() + " IsFull status has been updated",
                                    Date = DateTime.Now
                                };
                                await _logDataService.Create(newLog2);
                                suggestion.Remove(item);
                                break;
                            }
                            else
                            {
                                item.Count = item.Count - extra;
                                await _itemDataService.Update(item.Id, item);
                                var newLog = new Log
                                {
                                    Operation = "Item " + item.Id.ToString() + " from box " + item.fk_BoxId.ToString()+ " count has been updated",
                                    Date = DateTime.Now
                                };
                                await _logDataService.Create(newLog);
                                var box = boxes.FirstOrDefault(b => b.Id == item.fk_BoxId);
                                box.isFull = false;
                                await _boxDataService.Update(item.fk_BoxId, box);
                                var newLog2 = new Log
                                {
                                    Operation = "Box " + item.fk_BoxId.ToString() + " IsFull status has been updated",
                                    Date = DateTime.Now
                                };
                                await _logDataService.Create(newLog2);
                                break;
                            }
                        }
                        continue;
                    }
                }
                //OnSearchDone();
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<ItemType>> PopulateSearchResults(string searchText)
        {
            try
            {
                IEnumerable<ItemType> suggestions = await _itemTypeDataService.GetSuggestions(searchText);

                return suggestions.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static SearchViewModel LoadSearch(IBuyStockService stocks)
        {
            SearchViewModel majorIndexViewModel = new SearchViewModel(stocks);
            stocks.FindItems();
            stocks.BuyStock();
            return majorIndexViewModel;
        }
        public async void Initilization()
        {
            ItemList = await _stocks.FindItems();
            BoxList = await _stocks.BuyStock();
            ItemTypeList = await _stocks.FindTypes();
        }
    }
}

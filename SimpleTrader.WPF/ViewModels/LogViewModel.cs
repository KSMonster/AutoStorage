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
using System.Linq;
//using SimpleTrader.WPF.Commands.LogViewModelCommands;

namespace SimpleTrader.WPF.ViewModels {
    public class LogViewModel : ViewModelBase {

        private readonly IBuyStockService _stocks;
        private IEnumerable<Log> _loglist;
        private readonly LogDataService _logDataService;
        public IEnumerable<Log> LogList
        {
            get { return _loglist; }
            set
            {
                _loglist = value;
                OnPropertyChanged(nameof(LogList));
            }
        }

        private readonly ReplaySubject<Unit> _logDone = new ReplaySubject<Unit>(1);

        public IObservable<Unit> LogDone => _logDone;

        public void OnLogDone()
        {
            _logDone.OnNext(Unit.Default);
        }
        public LogViewModel(IBuyStockService stocks)
        {
            var dbContextFactory = new SimpleTraderDbContextFactory();
            _logDataService = new LogDataService(dbContextFactory);
            _stocks = stocks;
            Initilization();
        }
        public static LogViewModel LoadLogs(IBuyStockService stocks)
        {
            LogViewModel majorIndexViewModel = new LogViewModel(stocks);
            stocks.GetLogs();
            return majorIndexViewModel;
        }
        public async void Initilization()
        {
            LogList = (await _stocks.GetLogs()).OrderByDescending(log => log.Date).ToList();

        }
    }
}

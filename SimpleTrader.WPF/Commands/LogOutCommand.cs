using SimpleTrader.Domain.Core;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.EntityFramework;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.WPF.Controls;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Reactive;
using System.Windows;
using System.Windows.Input;

namespace SimpleTrader.WPF.Commands
{
    public class LogOutCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly IBuyStockService _stocks;
        private readonly HomeViewModel _loginViewModel;
        private IDisposable _loginSubscription;
        private readonly RoleDataService _roleDataService;
        private readonly LogDataService _logDataService;

        public LogOutCommand()
        {
            var dbContextFactory = new SimpleTraderDbContextFactory();
            _roleDataService = new RoleDataService(dbContextFactory);
            _logDataService = new LogDataService(dbContextFactory);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var newRole = new Role
            {
                Id = 1,
                role = -1
            };
            await _roleDataService.Update(1, newRole);
            var newLog = new Log
            {
                Operation = "User has logged out",
                Date = DateTime.Now
            };
            await _logDataService.Create(newLog);
            AppCore.Instance.Role = -1;
        }
    }
}

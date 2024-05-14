using Microsoft.AspNetCore.Identity;
using SimpleTrader.Domain.Core;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.WPF.Controls;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleTrader.WPF.Commands.LoginViewModelCommands {
    public class LoginViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly IBuyStockService _stocks;
        private readonly HomeViewModel _loginViewModel;
        private IDisposable _loginSubscription;

        public LoginViewModelCommand(IBuyStockService stocks, HomeViewModel loginViewModel) {
            _stocks = stocks;
            _loginViewModel = loginViewModel;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (_loginViewModel.Username == null || parameter as PasswordBox == null)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            string username = _loginViewModel.Username;
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox.Password;
            //var hashedPassword = new PasswordHasher<object?>().HashPassword(null, password);
            //MessageBox.Show(hashedPassword);

            var role = _loginViewModel.TryToLogin(username, password);
            int roleint = await role;

            AppCore.Instance.Role = roleint;
            _loginViewModel.OnLoginDone();
        }
    }
}

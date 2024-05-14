using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.EntityFramework;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.WPF.Commands.LoginViewModelCommands;
using SimpleTrader.WPF.Models;
using SimpleTrader.WPF.Controls;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.Commands;
using Microsoft.AspNetCore.Identity;


namespace SimpleTrader.WPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public LoginViewModel LoginViewModel { get; set; }

        private readonly IBuyStockService _stocks;
        private readonly UserDataService _userDataService;
        private readonly RoleDataService _roleDataService;
        private readonly LogDataService _logDataService;
        private string _userName;
        private string _password;
        private IEnumerable<User> _userlist;
        private IEnumerable<Role> _rolelist;
        public ICommand LoginCommand { get; }
        public ICommand LogOutCommand { get; }

        public string Username
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
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
        private readonly ReplaySubject<Unit> _loginDone = new ReplaySubject<Unit>(1);

        public IObservable<Unit> LoginDone => _loginDone;

        public void OnLoginDone()
        {
            _loginDone.OnNext(Unit.Default);
        }

        public HomeViewModel(LoginViewModel majorIndexListingViewModel)
        {
            var dbContextFactory = new SimpleTraderDbContextFactory();
            LoginViewModel = majorIndexListingViewModel;
            _userDataService = new UserDataService(dbContextFactory);
            _roleDataService = new RoleDataService(dbContextFactory);
            _logDataService = new LogDataService(dbContextFactory);
            LoginCommand = new LoginViewModelCommand(_stocks, this);
            OnLoginDone();
        }

        public void Update()
        {
            LoginViewModel.Initilization();
        }

        public async Task<int> TryToLogin(string username, string password)
        {
            UserList = await _userDataService.GetAllWhere(username);

            foreach (var user in UserList)
            {
                //user.Password = password;
                //await _userDataService.Update(user.Id, user);
                var passwordVerificationResult = new PasswordHasher<object?>().VerifyHashedPassword(null, user.Password, password);
                switch (passwordVerificationResult)
                {
                    case PasswordVerificationResult.Failed:
                        MessageBox.Show("Password incorrect.");
                        break;

                    case PasswordVerificationResult.Success:
                        //MessageBox.Show("Password ok.");
                        var newRole = new Role
                        {
                            Id = 1,
                            role = user.Role
                        };
                        await _roleDataService.Update(1, newRole);
                        var newLog = new Log
                        {
                            Operation = "User ID " + user.Id.ToString() + " has logged in ",
                            Date = DateTime.Now
                        };
                        await _logDataService.Create(newLog);
                        return newRole.role;
    
                    case PasswordVerificationResult.SuccessRehashNeeded:
                        MessageBox.Show("Password ok but should be rehashed and updated.");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return -1;

        }
    }
}

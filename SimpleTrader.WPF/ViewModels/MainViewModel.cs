using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimpleTrader.Domain.Core;
using SimpleTrader.Domain.Models;
using SimpleTrader.EntityFramework;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.WPF.Commands;
using SimpleTrader.WPF.Models;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SimpleTrader.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISimpleTraderViewModelFactory _viewModelFactory;

        public INavigator Navigator { get; set; }
        public ICommand UpdateCurrentViewModelCommand { get; }
        public ICommand LogOutCommand { get; }
        private readonly RoleDataService _roleDataService;
        private IEnumerable<Role> _rolelist;

        // Change Role property type to Task<int>
        public int Role => AppCore.Instance.Role;
        public Visibility AdminVisibility => AppCore.Instance.Role >= 1 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility LoginVisibility => AppCore.Instance.Role < 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility LogOutVisibility => AppCore.Instance.Role >= 0 ? Visibility.Visible : Visibility.Collapsed;

        public MainViewModel(INavigator navigator, ISimpleTraderViewModelFactory viewModelFactory)
        {
            Navigator = navigator;
            _viewModelFactory = viewModelFactory;
            var dbContextFactory = new SimpleTraderDbContextFactory();
            _roleDataService = new RoleDataService(dbContextFactory);

            // Updates the displayed View and corresponding ViewModel inside the ContentControl
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, _viewModelFactory);
            LogOutCommand = new LogOutCommand();
            
            // Await GetUserRoleFromDatabase method call
            AppCore.Instance.Role = GetUserRoleFromDatabase();
            AppCore.Instance.OnRoleChange += HandleOnLogin;
            /*AppCore.Instance.PropertyChanged += (sender, arg) => { 
                if (arg.PropertyName == nameof(AppCore.AdminVisibility))
                {
                    PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(AdminVisibility)));
                } };*/
            // Displays the HomeView when the application starts
            UpdateCurrentViewModelCommand.Execute(ViewType.Home);
        }

        private void HandleOnLogin()
        {
            OnPropertyChanged(nameof(AdminVisibility));
            OnPropertyChanged(nameof(LoginVisibility));
            OnPropertyChanged(nameof(LogOutVisibility));
        }

        private int GetUserRoleFromDatabase()
        {
            Role role = _roleDataService.GetRole(1);

            // Check if role is not null to avoid null reference exception
            if (role != null)
            {
                return role.role;
            }
            else
            {
                // Handle case where role with ID 1 is not found
                // You can throw an exception, return a default value, or handle it based on your application's requirements
                throw new Exception("Role with ID 1 not found");
            }
        }
    }
}

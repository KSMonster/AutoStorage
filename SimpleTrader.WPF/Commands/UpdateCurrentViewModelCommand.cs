using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Reactive;
using System.Windows.Input;

namespace SimpleTrader.WPF.Commands {
    public class UpdateCurrentViewModelCommand : ICommand, IDisposable
    {
        public event EventHandler CanExecuteChanged;

        private readonly INavigator _navigator;
        private readonly ISimpleTraderViewModelFactory _viewModelFactory;
        private IDisposable _observingEndedSubscription;

        public UpdateCurrentViewModelCommand(INavigator navigator, ISimpleTraderViewModelFactory viewModelFactory) {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;

                var viewModel = _viewModelFactory.CreateViewModel(viewType);
                int count = 0;
                // Check if the created ViewModel is DemoViewModel
                if (viewModel is PutViewModel putViewModel)
                {
                    // Subscribe to ObservingEnded event
                    _observingEndedSubscription = putViewModel.PutDone.Subscribe(OnPutDone);
                }
                if (viewModel is SearchViewModel searchViewModel)
                {
                    // Subscribe to ObservingEnded event
                    _observingEndedSubscription = searchViewModel.SearchDone.Subscribe(OnSearchDone);
                }
                if (viewModel is HomeViewModel homeViewModel&& count >= 1)
                {
                    // Subscribe to ObservingEnded event
                    _observingEndedSubscription = homeViewModel.LoginDone.Subscribe(OnLoginDone);
                }
                if (viewModel is CreateItemViewModel createItemViewModel)
                {
                    // Subscribe to ObservingEnded event
                    _observingEndedSubscription = createItemViewModel.CreateItemDone.Subscribe(OnCreateItemDone);
                }
                if (viewModel is CreateBoxesViewModel createBoxesViewModel)
                {
                    // Subscribe to ObservingEnded event
                    _observingEndedSubscription = createBoxesViewModel.CreateBoxesDone.Subscribe(OnCreateBoxesDone);
                }
                if (viewModel is BoxViewModel boxViewModel)
                {
                    // Subscribe to ObservingEnded event
                    _observingEndedSubscription = boxViewModel.BoxDone.Subscribe(OnBoxDone);
                }
                if (viewModel is LogViewModel logViewModel)
                {
                    // Subscribe to ObservingEnded event
                    _observingEndedSubscription = logViewModel.LogDone.Subscribe(OnLogDone);
                }
                count++;
                _navigator.CurrentViewModel = viewModel;
            }
        }
        private void OnPutDone(Unit unit)
        {
            // Update the Sessions in BenchmarksViewModel
            if (_viewModelFactory.CreateViewModel(ViewType.Put) is PutViewModel putViewModel)
            {
                putViewModel.Initilization();
            }
        }
        private void OnSearchDone(Unit unit)
        {
            // Update the Sessions in BenchmarksViewModel
            if (_viewModelFactory.CreateViewModel(ViewType.Box) is BoxViewModel boxViewModel)
            {
                boxViewModel.Initilization();
            }
        }
        private void OnCreateItemDone(Unit unit)
        {
            // Update the Sessions in BenchmarksViewModel
            if (_viewModelFactory.CreateViewModel(ViewType.Box) is BoxViewModel boxViewModel)
            {
                boxViewModel.Initilization();
            }
        }
        private void OnBoxDone(Unit unit)
        {
            // Update the Sessions in BenchmarksViewModel
            if (_viewModelFactory.CreateViewModel(ViewType.Box) is BoxViewModel boxViewModel)
            {
                boxViewModel.Initilization();
            }
        }
        private void OnLogDone(Unit unit)
        {
            // Update the Sessions in BenchmarksViewModel
            if (_viewModelFactory.CreateViewModel(ViewType.Logs) is LogViewModel logViewModel)
            {
                logViewModel.Initilization();
            }
        }
        private void OnCreateBoxesDone(Unit unit)
        {
            // Update the Sessions in BenchmarksViewModel
            if (_viewModelFactory.CreateViewModel(ViewType.Box) is BoxViewModel boxViewModel)
            {
                boxViewModel.Initilization();
            }

        }
        private void OnLoginDone(Unit unit)
        {
            // Update the Sessions in BenchmarksViewModel
            if (_viewModelFactory.CreateViewModel(ViewType.Login) is HomeViewModel homeViewModel)
            {
                homeViewModel.Update();
            }
        }

        public void Dispose()
        {
            _observingEndedSubscription?.Dispose();
        }
    }
}

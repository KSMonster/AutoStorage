using SimpleTrader.WPF.State.Navigators;
using System;

namespace SimpleTrader.WPF.ViewModels.Factories {
    public class SimpleTraderViewModelFactory : ISimpleTraderViewModelFactory {
        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
        private readonly CreateViewModel<PutViewModel> _createPutViewModel;
        private readonly CreateViewModel<BoxViewModel> _createBoxViewModel;
        private readonly CreateViewModel<SearchViewModel> _createSearchViewModel;
        private readonly CreateViewModel<CreateItemViewModel> _createItemViewModel;
        private readonly CreateViewModel<CreateBoxesViewModel> _createBoxesViewModel;
        private readonly CreateViewModel<LogViewModel> _logViewModel;

        public SimpleTraderViewModelFactory(CreateViewModel<HomeViewModel> createHomeViewModel,
            CreateViewModel<PutViewModel> createPutViewModel, 
            CreateViewModel<SearchViewModel> createSearchViewModel,
            CreateViewModel<CreateItemViewModel> createItemViewModel,
            CreateViewModel<CreateBoxesViewModel> createBoxesViewModel,
            CreateViewModel<LogViewModel> logViewModel,
            CreateViewModel<BoxViewModel> createBoxViewModel)
        {
            _createHomeViewModel = createHomeViewModel;
            _createPutViewModel = createPutViewModel;
            _createSearchViewModel = createSearchViewModel;
            _createItemViewModel = createItemViewModel;
            _createBoxesViewModel = createBoxesViewModel;
            _logViewModel = logViewModel;
            _createBoxViewModel = createBoxViewModel;
        }

        public ViewModelBase CreateViewModel(ViewType viewType) {
            switch (viewType) {
                case ViewType.Home:
                    return _createHomeViewModel();
                case ViewType.Put:
                    return _createPutViewModel();
                case ViewType.Box:
                    return _createBoxViewModel();
                case ViewType.Search:
                    return _createSearchViewModel();
                case ViewType.CreateItem:
                    return _createItemViewModel();
                case ViewType.CreateBoxes:
                    return _createBoxesViewModel();
                case ViewType.Logs:
                    return _logViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }
    }
}

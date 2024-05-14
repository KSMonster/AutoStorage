using SimpleTrader.WPF.Commands;
using SimpleTrader.WPF.State.Navigators;
using System.Windows.Input;

namespace SimpleTrader.WPF.ViewModels
{
    public class InspectViewModel : ViewModelBase
    {

        // Button that changes the View and ViewModel inside the ContentControl
        private ICommand _toHomeCommand;
        public ICommand ToHomeCommand
        {
            get { return _toHomeCommand; }
            set { _toHomeCommand = value; }
        }
        public InspectViewModel(IRenavigator renavigator)
        {
            ToHomeCommand = new ToHomeCommand(renavigator);
        }
        public BoxViewModel BoxViewModel { get; set; }

        public InspectViewModel(BoxViewModel majorIndexListingViewModel)
        {
            BoxViewModel = majorIndexListingViewModel;
        }
    }
}

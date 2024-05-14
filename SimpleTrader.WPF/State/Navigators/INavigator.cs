using SimpleTrader.WPF.ViewModels;

namespace SimpleTrader.WPF.State.Navigators {
    public enum ViewType {
        // What is passed in through the ICommand execute method, aka the parameter of the command
        Home,
        Put,
        Search,
        CreateItem,
        CreateBoxes,
        Logs,
        Login,
        Box
    }
    public interface INavigator {
        ViewModelBase CurrentViewModel { get; set; }
    }
}

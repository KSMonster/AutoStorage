using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.EntityFramework;
using SimpleTrader.FinancialModelingPrepAPI.Services;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTrader.WPF.HostBuilder
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<SimpleTraderDbContextFactory>();
                services.AddSingleton<IDataService<Account>, AccountDataService>();
                services.AddSingleton<IDataService<Box>, BoxDataService>();
                services.AddSingleton<IDataService<User>, UserDataService>();
                services.AddSingleton<IDataService<Item>, ItemDataService>();
                services.AddSingleton<IDataService<ItemType>, ItemTypeDataService>();
                services.AddSingleton<IDataService<Log>, LogDataService>();
                services.AddSingleton<IRoleService<Role>, RoleDataService>();
                services.AddSingleton<IItemDataService<Item>, ExtraItemDataService>();
                services.AddSingleton<IStockPriceService, StockPriceService>();
                services.AddSingleton<IBuyStockService, BuyStockService>();
                services.AddSingleton<IMajorIndexService, MajorIndexService>();

                services.AddSingleton<ISimpleTraderViewModelFactory, SimpleTraderViewModelFactory>();
                services.AddSingleton<BoxViewModel>();
                services.AddSingleton<PutViewModel>();
                services.AddSingleton<SearchViewModel>();
                services.AddSingleton<CreateItemViewModel>();
                services.AddSingleton<CreateBoxesViewModel>();
                services.AddSingleton<LogViewModel>();
                services.AddSingleton<LoginViewModel>();
            });

            return hostBuilder;
        }
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                #region HomeViewModel
                services.AddSingleton<HomeViewModel>(services => new HomeViewModel(
                    LoginViewModel.LoginViewMode(
                        services.GetRequiredService<IBuyStockService>())));
                services.AddSingleton<CreateViewModel<HomeViewModel>>(services => {
                    return () => services.GetRequiredService<HomeViewModel>();
                });
                #endregion

                #region BoxViewModel
                services.AddSingleton<BoxViewModel>(services => new BoxViewModel((
                        services.GetRequiredService<IBuyStockService>())));
                services.AddSingleton<CreateViewModel<BoxViewModel>>(services => {
                    return () => services.GetRequiredService<BoxViewModel>();
                });
                #endregion

                #region PutViewModels

                services.AddSingleton<PutViewModel>(services => new PutViewModel((
                        services.GetRequiredService<IBuyStockService>())));
                services.AddSingleton<CreateViewModel<PutViewModel>>(services => {
                    return () => services.GetRequiredService<PutViewModel>();
                });
                #endregion

                #region SearchViewModel
                services.AddSingleton<SearchViewModel>(services => new SearchViewModel(
                        services.GetRequiredService<IBuyStockService>()));

                services.AddSingleton<CreateViewModel<SearchViewModel>>(services => {
                    return () => services.GetRequiredService<SearchViewModel>();
                });
                #endregion

                #region CreateItemViewModel
                services.AddSingleton<CreateItemViewModel>(services => new CreateItemViewModel((
                    services.GetRequiredService<IBuyStockService>())));
                services.AddSingleton<CreateViewModel<CreateItemViewModel>>(services => {
                    return () => services.GetRequiredService<CreateItemViewModel>();
                });
                #endregion

                #region CreateBoxesViewModel
                services.AddSingleton<CreateBoxesViewModel>(services => new CreateBoxesViewModel((
                    services.GetRequiredService<IBuyStockService>())));
                services.AddSingleton<CreateViewModel<CreateBoxesViewModel>>(services => {
                    return () => services.GetRequiredService<CreateBoxesViewModel>();
                });
                #endregion

                #region LogViewModel
                services.AddSingleton<LogViewModel>(services => new LogViewModel((
                    services.GetRequiredService<IBuyStockService>())));
                services.AddSingleton<CreateViewModel<LogViewModel>>(services => {
                    return () => services.GetRequiredService<LogViewModel>();
                });
                #endregion

                #region AddScoped?
                services.AddScoped<INavigator, Navigator>();
                services.AddScoped<MainViewModel>();
                services.AddScoped<BoxViewModel>();
                services.AddScoped<PutViewModel>();
                services.AddScoped<CreateItemViewModel>();
                services.AddScoped<CreateBoxesViewModel>();
                services.AddScoped<LogViewModel>();
                services.AddScoped<LoginViewModel>();

                services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
                #endregion
            });

            return hostBuilder;
        }

    }
}

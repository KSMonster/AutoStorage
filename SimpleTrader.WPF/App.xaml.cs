using Microsoft.Extensions.DependencyInjection;
using SimpleTrader.Domain.Core;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.EntityFramework;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.FinancialModelingPrepAPI.Services;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System;
using System.Globalization;
using Microsoft.Extensions.Hosting;
using System.Windows;
using SimpleTrader.WPF.HostBuilder;

namespace SimpleTrader.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .AddServices()
                .AddViewModels()
                .Build();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {

            AppCore appcore = AppCore.Instance;

            _host.Start();
            Window window = _host.Services.GetRequiredService<MainWindow>();

            window.Show();

            base.OnStartup(e);
        }
    }
}

using SimpleTrader.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleTrader.Domain.Core
{
    public sealed class AppCore : IDisposable
    {

        /// <summary>
        /// Vienintelis AppCore klases objektas (instance)
        /// </summary>
        private static readonly AppCore _instance = new AppCore();

        /// <summary>D
        /// [Singleton] AppCore klases objektas (vienintelis instance)
        /// </summary>
        // ReSharper disable once ConvertToAutoProperty
        public static AppCore Instance => _instance;

        public Action OnRoleChange;
        private int _role;
        public int Role { get=>_role; set { 
                _role = value;
                OnRoleChange?.Invoke();
            } }
        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled; set
            {
                _isEnabled = value;
            }
        }
        public bool AdminVisibility => Role >= 1 ? true : false;
        public bool LoginVisibility => Role < 0 ? true : false;
        public bool LogOutVisibility => Role >= 0 ? true : false;
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}

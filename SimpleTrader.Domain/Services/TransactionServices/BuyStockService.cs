using SimpleTrader.Domain.Exceptions;
using SimpleTrader.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleTrader.Domain.Services.TransactionServices {
    public class BuyStockService : IBuyStockService {
        private readonly IStockPriceService _stockPriceService;
        private readonly IDataService<Account> _accountService;
        //private readonly IDataService<User> _userService;
        private readonly IDataService<Box> _boxService;
        private readonly IDataService<Item> _itemService; 
        private readonly IDataService<ItemType> _itemTypeService;
        private readonly IDataService<Log> _logService;
        private readonly IRoleService<Role> _roleService;

        public BuyStockService(IDataService<Box> boxService, IStockPriceService stockPriceService, IDataService<Account> accountService, IDataService<Item> itemService, IDataService<ItemType> itemTypeService, IDataService<Log> logService, IRoleService<Role> roleService)
        {
            _itemTypeService = itemTypeService;
            _boxService = boxService;
            //_userService = userService;
            _stockPriceService = stockPriceService;
            _accountService = accountService;
            _itemService = itemService;
            _logService = logService;
            _roleService = roleService;
        }
        public async Task<IEnumerable<ItemType>> FindTypes()
        {
            return await _itemTypeService.GetAll();
        }
        public async Task<IEnumerable<Item>> FindItems()
        {
            return await _itemService.GetAll();
        }
        public async Task<IEnumerable<Role>> FindRole()
        {
            return await _roleService.GetAll();
        }

        public async Task<IEnumerable<Log>> GetLogs()
        {
            return await _logService.GetAll();
        }

        public async Task<IEnumerable<Box>> BuyStock()
        {

            return await _boxService.GetAll();

        }
    }
}

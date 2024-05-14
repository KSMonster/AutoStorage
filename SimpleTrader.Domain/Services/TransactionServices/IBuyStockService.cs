using SimpleTrader.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleTrader.Domain.Services.TransactionServices {
    public interface IBuyStockService {
        Task<IEnumerable<Box>> BuyStock();
        Task<IEnumerable<Item>> FindItems();
        Task<IEnumerable<ItemType>> FindTypes();
        //Task<IEnumerable<User>> FindUsersBy(string a, string b);
        Task<IEnumerable<Log>> GetLogs();
    }
}

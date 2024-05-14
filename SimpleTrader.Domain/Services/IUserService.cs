using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleTrader.Domain.Services {
    // The <T> means that it is generic, aka that the T is a placeholder for future parameters when the interface is used, and can take on different data types
    public interface IUserService<T> {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllWhere(string a, string b);
        Task<T> Get(int id);
        Task<T> Create(T entity);
        Task<T> Update(int id, T entity);
        Task<bool> Delete(int id);
    }
}

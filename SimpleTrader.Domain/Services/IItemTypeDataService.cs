﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleTrader.Domain.Services {
    // The <T> means that it is generic, aka that the T is a placeholder for future parameters when the interface is used, and can take on different data types
    public interface IItemTypeDataService<T> {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetWhere(string name);
        Task<IEnumerable<T>> GetSuggestions(string name);
        Task<T> Create(T entity);
        Task<T> Update(int id, T entity);
        Task<bool> Delete(int id);
    }
}

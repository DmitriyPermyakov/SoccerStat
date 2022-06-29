using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoccerStatResourceServer.Repository
{
    public interface IRepository<T> where T : class
    {
        public Task<T> CreateAsync(T entity);
        public Task<List<T>> GetAllAsync();
        public Task<T> GetByIdAsync(string id);
        public void Update(T entity);
        public void Delete(T entity);
        public Task SaveAsync();
    }
}

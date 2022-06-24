using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoccerStatResourceServer.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ResourceDbContext context;
        private DbSet<T> dbSet;
        public Repository(ResourceDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }
        public void Create(T entity)
        {
            dbSet.Add(entity);
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public async Task SaveAsync()
        {
           _ = await context.SaveChangesAsync();
        }
    }
}

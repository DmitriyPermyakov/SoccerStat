using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data;


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
        public async Task<T> CreateAsync(T entity)
        {
            var createdEntity = await dbSet.AddAsync(entity);
            return createdEntity.Entity;
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }
        
        public async Task<T> GetByIdAsync(string id)
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

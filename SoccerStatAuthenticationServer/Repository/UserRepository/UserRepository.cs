using Microsoft.EntityFrameworkCore;
using SoccerStatAuthenticationServer.DTOs.DomainObjects;
using SoccerStatAuthenticationServer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private AuthenticationServerDbContext context;
        public UserRepository(AuthenticationServerDbContext ctx)
        {
            context = ctx;
        }
        public async Task<User> Create(User user)
        {
            var createdUser = await context.Users.AddAsync(user);
            _ = await context.SaveChangesAsync();
            return createdUser.Entity;
        }

        public async Task<string> Delete(User user)
        {
            context.Users.Remove(user);
            _ = await context.SaveChangesAsync();

            return "User deleted";
        }

        public async Task<List<User>> GetAll()
        {
            List<User> users = await context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetByEmail(string email)
        {
            User user = await context.Users.Where(u => u.Email == email).FirstAsync();
            return user;
        }

        public async Task<User> GetById(int id)
        {
            User user = await context.Users.FindAsync(id);
            return user;
        }

        public async Task<User> Update(User user)
        {
           var updatedUser = context.Users.Update(user);
            _ = await context.SaveChangesAsync();
            return updatedUser.Entity;
        }
    }
}

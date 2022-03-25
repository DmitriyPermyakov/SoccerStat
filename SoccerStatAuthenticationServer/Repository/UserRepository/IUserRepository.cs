using SoccerStatAuthenticationServer.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Repository.UserRepository
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllAsync();
        public Task<User> GetByIdAsync(int id);
        public Task<User> GetByEmailAsync(string Email);
        public Task<User> CreateAsync(User user);
        public Task<User> UpdateAsync(User user);
        public Task<string> DeleteAsync(User user);

    }
}

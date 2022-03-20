using SoccerStatAuthenticationServer.DTOs.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Repository.UserRepository
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAll();
        public Task<User> GetById(int id);
        public Task<User> GetByEmail(string Email);
        public Task<User> Create(User user);
        public Task<User> Update(User user);
        public Task<string> Delete(User user);

    }
}

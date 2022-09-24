using SoccerStatAuthenticationServer.DomainObjects;
using SoccerStatAuthenticationServer.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Services.UserService
{
    public interface IUserService
    {
        public Task<User> CreateAsync(UserRequest userRequest);
        public Task UpdateAsync(Guid id, UserRequest userRequest);
        public Task<List<User>> GetAllAsync();
        public Task<User> GetByIdAsync(Guid id);
        public Task<User> GetByEmailAsync(string email);
        public Task DeleteAsync(Guid id);
    }

}

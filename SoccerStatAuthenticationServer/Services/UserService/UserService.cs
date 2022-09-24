using SoccerStatAuthenticationServer.DomainObjects;
using SoccerStatAuthenticationServer.DTOs.Requests;
using SoccerStatAuthenticationServer.Exceptions;
using SoccerStatAuthenticationServer.Repository.UserRepository;
using SoccerStatAuthenticationServer.Services.PasswordHasher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Services.UserService
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        private IPasswordHasher passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
        }

        public async Task<User> CreateAsync(UserRequest userRequest)
        {
            User user = await userRepository.GetByEmailAsync(userRequest.Email);
            if (user != null)
                throw new UserExistsException("User already exists");

            User newUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = userRequest.Email,
                PasswordHash = passwordHasher.HashPassword(userRequest.Password),
                Roles = userRequest.Roles
            };

            await userRepository.CreateAsync(newUser);

            return newUser;
        }

        public async Task DeleteAsync(Guid id)
        {
            User user = await userRepository.GetByIdAsync(id);
            if (user == null)
                throw new UserNotExistsException();
            await userRepository.DeleteAsync(user);
        }

        public async Task<List<User>> GetAllAsync()
        {
            List<User> users = await userRepository.GetAllAsync();
            return users;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            User user = await userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new UserNotExistsException();

            return user;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            User user = await userRepository.GetByIdAsync(id);
            if (user == null)
                throw new UserNotExistsException();

            return user;
        }

        public async Task UpdateAsync(Guid id, UserRequest userRequest)
        {
            User user = await userRepository.GetByIdAsync(id);
            if (user == null)
                throw new UserNotExistsException();

            user.Email = userRequest.Email;
            user.PasswordHash = passwordHasher.HashPassword(userRequest.Password);
            user.Roles = userRequest.Roles;

            await userRepository.UpdateAsync(user);
        }
    }
}

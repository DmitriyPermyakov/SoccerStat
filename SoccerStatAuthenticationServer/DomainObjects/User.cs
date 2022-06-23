using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.DomainObjects
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Role> Roles { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
    }
    
    public enum Role
    {
        Admin,
        User
    }
}

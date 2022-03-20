using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.DTOs.DomainObjects
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RefreshTokenId { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}

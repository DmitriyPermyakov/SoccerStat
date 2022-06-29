using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SoccerStatAuthenticationServer.DomainObjects;

namespace SoccerStatAuthenticationServer.DTOs.Requests
{
    public class UserRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public ICollection<Role> Roles { get; set; }
    }    
}

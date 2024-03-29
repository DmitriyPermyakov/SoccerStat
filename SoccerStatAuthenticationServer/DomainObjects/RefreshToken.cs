﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.DomainObjects
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }       
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}

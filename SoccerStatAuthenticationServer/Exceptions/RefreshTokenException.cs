using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Exceptions
{
    public class RefreshTokenException : Exception
    {
        public RefreshTokenException() { }
        public RefreshTokenException(string message) : base(message) { }
        public RefreshTokenException(string message, Exception inner) : base(message, inner) { }
    }
}

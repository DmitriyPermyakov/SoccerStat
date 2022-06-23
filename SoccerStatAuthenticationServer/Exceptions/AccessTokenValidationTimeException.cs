using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Exceptions
{
    public class AccessTokenValidationTimeException : Exception
    {
        public AccessTokenValidationTimeException() { }
        public AccessTokenValidationTimeException(string message) : base(message) { }
        public AccessTokenValidationTimeException(string message, Exception inner) : base(message, inner) { }
    }
}

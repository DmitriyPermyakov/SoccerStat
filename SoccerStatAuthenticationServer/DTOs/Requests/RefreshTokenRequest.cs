﻿
namespace SoccerStatAuthenticationServer.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
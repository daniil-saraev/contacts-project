﻿using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Requests
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }

        public string UserId { get; set; }
    }
}

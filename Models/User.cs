﻿using Microsoft.AspNetCore.Identity;

namespace CatalogoApi.Models
{
    public class User : IdentityUser
    {

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;

namespace ApiTrello.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; } = null!;
        public string? Salt { get; set; }
        public string? FcmToken { get; set; }

    }
}

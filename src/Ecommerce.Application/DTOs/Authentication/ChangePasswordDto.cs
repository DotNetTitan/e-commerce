﻿namespace Ecommerce.Application.DTOs.Authentication
{
    public class ChangePasswordDto
    {
        public required string Username { get; set; }
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
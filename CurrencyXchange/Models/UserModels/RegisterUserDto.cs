﻿namespace CurrencyXchange.Models.UserModels
{
    public class RegisterUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int CurrencyId { get; set; }
    }
}

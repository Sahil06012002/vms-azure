﻿namespace VendorManagementSystem.Application.IRepository
{
    public interface IUserTokenRepository
    {
        public int AdDtoken(string email, string token);
        public string? GetToken(string email);
        public int DeleteToken(string email);
    }
}

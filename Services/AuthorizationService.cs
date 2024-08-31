using AutoSql.Consts;
using System.Collections.Generic;

namespace AutoSql.Services
{
    public class AuthorizationService
    {
        private readonly Dictionary<string, string> _appUsers;

        public AuthorizationService()
        {
            _appUsers = UserCredentials.AppUsers; // Використовуємо дані з класу UserCredentials
        }

        public bool Login(string username, string password)
        {
            return _appUsers.ContainsKey(username) && _appUsers[username] == password;
        }
    }
}
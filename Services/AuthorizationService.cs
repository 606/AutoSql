using AutoSql.Consts;
using System.Collections.Generic;

namespace AutoSql.Services
{
    public class AuthorizationService
    {
        private readonly Dictionary<string, string> _appUsers = UserCredentials.AppUsers;

        public bool Login(string username, string password)
        {
            return _appUsers.ContainsKey(username) && _appUsers[username] == password;
        }
    }
}
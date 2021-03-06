using System.Collections.Generic;
using System.Linq;
using Shop.Models;

namespace Shop.Repositories
{
    public static class UserRepository
    {
        public static User Get(string username, string password)
        {   
            var users = new List<User>();

            users.Add(new User {Id = 1, Username = "anderson", Password = "anderson", Role = "gerente"});
            users.Add(new User {Id = 2, Username = "gabriel", Password = "gabriel", Role = "funcionario"});

            return users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password.ToLower() == password.ToLower()).FirstOrDefault();
        }
    }
}
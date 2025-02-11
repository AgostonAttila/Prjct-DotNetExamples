using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthTest.Models
{
    public class UserSession
    {
        public string? UserName { get; set; }
        public string? Role { get; set; }

        public UserSession(string userName,string role)
        {
            UserName = userName;
            Role = role;            
        }
    }
}

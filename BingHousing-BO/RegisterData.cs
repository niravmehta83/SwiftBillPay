using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingHousing_BO
{

    public class RegisterData
    {

        public string UserName { get; set; }

        public string Password2 { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        public RegisterData(string username, string password, string password2, string email)
        {
            this.UserName = username;
            this.Password2 = password2;
            this.Email = email;
            this.Password = password;
    }

    }
}

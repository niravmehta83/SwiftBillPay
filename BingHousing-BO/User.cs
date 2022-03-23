using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingHousing_BO
{

    public abstract class User
    {
        private int _userid;

        private bool _islockedOut;

        public int UserId
        {
            get
            {
                return _userid;
            }
            set
            {
                _userid = value;
            }
        }
        public bool IsLocked
        {
            get
            {
                return _islockedOut;
            }
            set
            {
                _islockedOut = value;
            }
        }


    }
    public class AccountUserModel : User
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
        private string _rolename;
        public string RoleName
        {
            get
            {
                return _rolename;
            }

            set
            {
                _rolename = value;
            }
        }
        
    }
}

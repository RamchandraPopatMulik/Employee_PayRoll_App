using Employee_Payroll_Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employee_Payroll_Manager.Interface
{
    public interface IUserManager
    {
        public UserModel Register(UserModel userModel);

        public string Login(string EmailID, string Password);
    }
}

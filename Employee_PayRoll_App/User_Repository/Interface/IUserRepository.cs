using Employee_Payroll_Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employee_Payroll_Repository.Interface
{
    public interface IUserRepository
    {
        public UserModel Register(UserModel userModel);

        public string Login(string EmailID, string Password);
    }
}

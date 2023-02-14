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
        public User_Ticket CreateTicketForPassword(string emailID, string token);

        public string ForgotPassword(string emailID);

        public bool ResetPassword(string Password, string emailID);
        public List<UserModel> GetAllUser();
    }
}

using Employee_Payroll_Manager.Interface;
using Employee_Payroll_Model;
using Employee_Payroll_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employee_Payroll_Manager.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository userRepository;

        public UserManager(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public UserModel Register(UserModel userModel)
        {
            try
            {
                return this.userRepository.Register(userModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Login(string EmailID, string Password)
        {
            try
            {
                return this.userRepository.Login(EmailID, Password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string ForgotPassword(string emailID)
        {
            try
            {
                return this.userRepository.ForgotPassword(emailID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool ResetPassword(string Password, string emailID)
        {
            try
            {
                return this.userRepository.ResetPassword(Password,emailID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

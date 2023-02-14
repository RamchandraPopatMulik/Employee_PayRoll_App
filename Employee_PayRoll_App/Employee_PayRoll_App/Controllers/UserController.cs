using Employee_Payroll_Manager.Interface;
using Employee_Payroll_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Employee_PayRoll_App.Controllers
{
    [Route("Employee_PayRoll/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;

        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }
        [HttpPost]
        [Route("Employee_PayRoll/Register")]
        public IActionResult Register(UserModel userModel)
        {
            try
            {
                UserModel registrationData = this.userManager.Register(userModel);
                if (registrationData != null)
                {
                    return this.Ok(new { success = true, message = "Registration Successfull", result = registrationData });
                }
                return this.Ok(new { success = true, message = "User Already Exists" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        [Route("Employee_PayRoll/Login")]
        public IActionResult Login(string EmailID, string Password)
        {
            try
            {
                string userToken = this.userManager.Login(EmailID, Password);
                if (userToken != null)
                {
                    return this.Ok(new { success = true, message = "Login Successfull", result = userToken });
                }
                return this.Ok(new { success = true, message = "Enter Valid EmailID or Password" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        [Route("Employee_PayRoll/ForgotPassword")]
        public IActionResult ForgotPassword(string emailID)
        {
            try
            {
                string emailToken = this.userManager.ForgotPassword(emailID);
                if (emailToken != null)
                {
                    return this.Ok(new { success = true, message = "Forgot Successfull", result = emailToken });
                }
                return this.Ok(new { success = true, message = "Enter Valid EmailID " });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPut]
        [Route("Employee_PayRoll/ResetPassword")]
        public IActionResult ResetPassword(string password, string confirmPassword)
        {
            try
            {
                string emailID = User.FindFirst(ClaimTypes.Email).Value.ToString();
                if (password == confirmPassword)
                {
                    bool userPassword = this.userManager.ResetPassword(password, emailID);
                    if (userPassword)
                    {
                        return this.Ok(new { success = true, message = "Password Reset Successfully", result = userPassword });
                    }
                }
                return this.Ok(new { success = true, message = "Enter Password same as above" });

            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        [Route("EmployeePayroll/GetAllUser")]
        public IActionResult GetAllUser()
        {
            try
            {
                List<UserModel> allUser = this.userManager.GetAllUser();
                if (allUser != null)
                {
                    return this.Ok(new { success = true, message = "All User Get Successfully", result = allUser });
                }
                return this.Ok(new { success = true, message = "No User Present in Database" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

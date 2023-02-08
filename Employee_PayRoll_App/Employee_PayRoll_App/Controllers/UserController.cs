using Employee_Payroll_Manager.Interface;
using Employee_Payroll_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
    }
}

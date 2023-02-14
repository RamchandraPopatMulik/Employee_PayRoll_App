using Employee_Payroll_Manager.Interface;
using Employee_Payroll_Model;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace Employee_PayRoll_App.Controllers
{
    [Route("EmployeePayroll/[controller]")]
    [ApiController]
    public class User_TicketController : ControllerBase
    {
        private readonly IBus bus;
        private readonly IUserManager userManager;

        public User_TicketController(IBus bus, IUserManager userManager)
        {
            this.bus = bus;
            this.userManager = userManager;
        }
        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> CreateTicketForPassword(string EmailID)
        {
            try
            {
                if (EmailID != null)
                {
                    var token = this.userManager.ForgotPassword(EmailID);
                    if (!string.IsNullOrEmpty(token))
                    {
                        User_Ticket userTicket = this.userManager.CreateTicketForPassword(EmailID, token);
                        Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                        var endPoint = await this.bus.GetSendEndpoint(uri);
                        await endPoint.Send(userTicket);
                        return Ok(new { sucess = true, message = "Email Sent Successfully" });
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "EmailID not Registered" });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went Wrong" });
                }
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}


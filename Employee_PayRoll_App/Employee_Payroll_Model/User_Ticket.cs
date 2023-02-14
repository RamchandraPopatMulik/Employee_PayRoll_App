using System;
using System.Collections.Generic;
using System.Text;

namespace Employee_Payroll_Model
{
    public class User_Ticket
    {
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string Token { get; set; }
        public DateTime IssueAt { get; set; }
    }
}

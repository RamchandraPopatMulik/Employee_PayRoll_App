using System;
using System.Collections.Generic;
using System.Text;

namespace Employee_Payroll_Model
{
    public class EmployeeModel
    {
        public string EmployeeName { get; set; }
        public DateTime Start_Date { get; set; }

        public string Gender { get; set; }

        public string Departments { get; set; }
        public long MobileNumber { get; set; }
        public string Address { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public decimal Basic_Salary { get; set; }

        public decimal Other_allowances { get; set; }

        public decimal Other_Income { get; set; }

        public decimal Professional_Tax { get; set; }
    }
}

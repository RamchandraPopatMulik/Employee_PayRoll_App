﻿using Employee_Payroll_Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employee_Payroll_Repository.Interface
{
    public interface IEmployeeRepository
    {
        public EmployeeModel AddEmployee(EmployeeModel employeeModel);
        public EmployeeModel UpdateEmployee(int EmployeeID, EmployeeModel employeeModel);
        public bool DeleteEmployee(int EmployeeID);
        public List<GetEmployeeModel> GetAllEmployee();
        public GetEmployeeModel GetEmployeeByID(int EmployeeID);
    }
}

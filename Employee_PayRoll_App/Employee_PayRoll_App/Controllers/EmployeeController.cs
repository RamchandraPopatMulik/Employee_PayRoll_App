using Employee_Payroll_Manager.Interface;
using Employee_Payroll_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Employee_PayRoll_App.Controllers
{
    [Route("Employee_Payroll/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeManager employeeManager;
        public EmployeeController(IEmployeeManager employeeManager)
        {
            this.employeeManager = employeeManager;
        }
        [HttpPost]
        [Route("BookStore/AddBook")]
        public IActionResult AddEmployee(EmployeeModel employeeModel)
        {
            try
            {
                EmployeeModel bookData = this.employeeManager.AddEmployee(employeeModel);
                if (bookData != null)
                {
                    return this.Ok(new { success = true, message = "Employee Added Successfully", result = bookData });
                }
                return this.Ok(new { success = true, message = "Employee Name Already Exists" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPut]
        [Route("EmployeePayroll/UpdateEmployee")]
        public IActionResult UpdateEmployee(int EmployeeID, EmployeeModel employeeModel)
        {
            try
            {
                EmployeeModel employeeData = this.employeeManager.UpdateEmployee(EmployeeID, employeeModel);
                if (employeeData != null)
                {
                    return this.Ok(new { success = true, message = "Employee Updated Successfully", result = employeeData });
                }
                return this.Ok(new { success = true, message = "Employee Not Updated" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpDelete]
        [Route("EmployeePayroll/DeleteEmployee")]
        public IActionResult DeleteEmployee(int EmployeeID)
        {
            try
            {
                bool deleteEmployee = this.employeeManager.DeleteEmployee(EmployeeID);
                if (deleteEmployee)
                {
                    return this.Ok(new { success = true, message = "Employee Deleted Successfully", result = deleteEmployee });
                }
                return this.Ok(new { success = true, message = "Employee Not Deleted" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        [Route("EmployeePayroll/GetAllEmployee")]
        public IActionResult GetAllEmployee()
        {
            try
            {
                List<GetEmployeeModel> allEmployess = this.employeeManager.GetAllEmployee();
                if (allEmployess != null)
                {
                    return this.Ok(new { success = true, message = "All Employee Get Successfully", result = allEmployess });
                }
                return this.Ok(new { success = true, message = "No Employee Present in Database" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        [Route("EmployeePayroll/GetEmployeeByID")]
        public IActionResult GetEmployeeByID(int EmployeeID)
        {
            try
            {
                GetEmployeeModel employee = this.employeeManager.GetEmployeeByID(EmployeeID);
                if (employee != null)
                {
                    return this.Ok(new { success = true, message = "Employee Get Successfully", result = employee });
                }
                return this.Ok(new { success = true, message = "Enter Valid Employee ID" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface IEmployeeService
    {
		Task<EmployeeDto> GetEmployeeById(int employeeId);
		Task<List<EmployeeDto>> GetAllEmployee(EmployeeDto employeeDto);
		Task<string> AddEmployee(EmployeeDto employeeDto);
		Task<string> UpdateEmployee(EmployeeDto employeeDto,int employeeId);
		Task<int> DeleteEmployee(int employeeId);
	}
}

using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
   public interface IEmployeeAdaptor
    {
        Task<EmployeeDto> GetEmployeeByIdAsync(int employeeId);
        Task<List<EmployeeDto>> GetAllEmployeeAsync();
		Task<string> AddEmployeeAsync(EmployeeDto employeeDto);
		Task<string> UpdateEmployeeAsync(EmployeeDto employeeDto,int employeeId);
		Task<int> DeleteEmployeeAsync(int employeeId);
	}
}

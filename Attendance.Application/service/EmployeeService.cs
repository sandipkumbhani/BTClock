using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;

namespace Attendance.Application.service
{
   public class EmployeeService : IEmployeeService
	{
		private readonly IEmployeeAdaptor _employeeAdaptor;
		public EmployeeService(IEmployeeAdaptor employeeAdaptor)
		{
			_employeeAdaptor = employeeAdaptor;
		}
		public async Task<EmployeeDto> GetEmployeeById(int employeeId)
		{
			return await _employeeAdaptor.GetEmployeeByIdAsync(employeeId);
		}
		public async Task<List<EmployeeDto>> GetAllEmployee()
		{
			return await _employeeAdaptor.GetAllEmployeeAsync();
			}
		public async Task<string> AddEmployee(EmployeeDto employeeDto)
		{
			return await _employeeAdaptor.AddEmployeeAsync(employeeDto);
		}
		public async Task<string> UpdateEmployee(EmployeeDto employeeDto,int employeeiId)
		{
			return await _employeeAdaptor.UpdateEmployeeAsync(employeeDto,employeeiId);
			}
		public async Task <int> DeleteEmployee(int employeeId)
		{
			return await _employeeAdaptor.DeleteEmployeeAsync(employeeId);
		}
	}
}

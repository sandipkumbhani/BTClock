using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface IDepartmentService
    {
		Task<DepartmentDto> GetDepartmentById(int departmentId);
		Task<List<DepartmentDto>> GetAllDepartments();
		Task<string> AddDepartment(DepartmentDto departmentDto);
		Task<string> UpdateDepartment(DepartmentDto departmentDto, int departmentId);
		Task<int> DeleteDepartment(int departmentId);
	}
}

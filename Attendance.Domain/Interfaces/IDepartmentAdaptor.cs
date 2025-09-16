using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface IDepartmentAdaptor
    {
		Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId);
		Task<List<DepartmentDto>> GetAllDepartmentsAsync();
        Task<string> AddDepartmentAsync(DepartmentDto departmentDto);
		Task<string> UpdateDepartmentAsync(DepartmentDto departmentDto, int departmentId);
		Task<int> DeleteDepartmentAsync(int departmentId);
	}
}

using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.service
{
   public class DepartmentService:IDepartmentService
    {
        private readonly IDepartmentAdaptor _departmentAdaptor;
		public DepartmentService(IDepartmentAdaptor departmentAdaptor)
		{
			_departmentAdaptor = departmentAdaptor;
		}
		public async Task<List<DepartmentDto>> GetAllDepartments()
		{
			return await _departmentAdaptor.GetAllDepartmentsAsync();
		}
		public async Task<DepartmentDto> GetDepartmentById(int Id)
		{
			return await _departmentAdaptor.GetDepartmentByIdAsync(Id);
		}
		public async Task<string> AddDepartment(DepartmentDto departmentDto)
		{
			return await _departmentAdaptor.AddDepartmentAsync(departmentDto);
		}
		public async Task<string> UpdateDepartment(DepartmentDto departmentDto, int Id)
		{
			return await _departmentAdaptor.UpdateDepartmentAsync(departmentDto, Id);
		}
		public async Task<int> DeleteDepartment(int Id)
		{
			return await _departmentAdaptor.DeleteDepartmentAsync(Id);
		}
	}
}

using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface IPermissionService
    {
		Task<List<PermissionDto>> GetAllPermissions();
		Task<PermissionDto> GetPermissionById(int permissionId);
		Task<string> AddPermission(PermissionDto permissionDto);
		Task<string> UpdatePermission(PermissionDto permissionDto, int permissionId);
		Task<int> DeletePermission(int permissionId);
	}
}

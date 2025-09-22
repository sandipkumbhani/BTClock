using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface IPermissionAdaptor
    {
        Task<PermissionDto> GetPermissionByIdAsync(int permissionId);
		Task<List<PermissionDto>> GetAllPermissionsAsync();
		Task<string> AddPermissionAsync(PermissionDto permissionDto);
		Task<string> UpdatePermissionAsync(PermissionDto permissionDto, int permissionId);
		Task<int> DeletePermissionAsync(int permissionId);
	}
}

using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.service
{
    public class PermissionService: IPermissionService
	{
        private readonly IPermissionAdaptor _permissionAdaptor;
		public PermissionService(IPermissionAdaptor permissionAdaptor)
		{
			_permissionAdaptor = permissionAdaptor;
		}
		public async Task<List<PermissionDto>> GetAllPermissions()
		{
			return await _permissionAdaptor.GetAllPermissionsAsync();
		}
		public async Task<PermissionDto> GetPermissionById(int permissionId)
		{
			return await _permissionAdaptor.GetPermissionByIdAsync(permissionId);
		}
		public async Task<string> AddPermission(PermissionDto permissionDto)
		{
			return await _permissionAdaptor.AddPermissionAsync(permissionDto);
		}
		public async Task<string> UpdatePermission(PermissionDto permissionDto, int permissionId)
		{
			return await _permissionAdaptor.UpdatePermissionAsync(permissionDto, permissionId);
		}
		public async Task<int> DeletePermission(int permissionId)
		{
			return await _permissionAdaptor.DeletePermissionAsync(permissionId);
		}
	}
}

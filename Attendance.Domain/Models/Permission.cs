using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
   public class Permission
    {
        public Permission()
		{
			permissionDto = new List<PermissionDto>();
		}
		public List<PermissionDto> permissionDto { get; set; }

	}
	public class PermissionDto
	{
		public int PermissionId { get; set; }
		[ForeignKey("DesignationId")]
		public int DesignationId { get; set; }
		public DesignationDto? Designation { get; set; }
		[ForeignKey("ModuleMasterId")]
		public int ModuleMasterId { get; set; }
		public ModuleMasterDto? ModuleMaster { get; set; }
		public bool CanAccess { get; set; }
	}
}

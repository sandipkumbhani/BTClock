using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class Role
    {
        public Role()
        {
            roleDto = new List<RoleDto>();
        }

        public List<RoleDto> roleDto { get; set; }
    }

    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}

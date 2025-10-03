using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAll();
    }
}

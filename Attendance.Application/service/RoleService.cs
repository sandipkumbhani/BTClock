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
    public class RoleService : IRoleService
    {
        private readonly IRoleAdaptor _adaptor;
        public RoleService(IRoleAdaptor adaptor)
        {
            _adaptor = adaptor;
        }
        public async Task<IEnumerable<RoleDto>> GetAll()
        {
            return await _adaptor.GetAllAsync();
        }
    }
}

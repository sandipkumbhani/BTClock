using Attendance.Domain.Models;

namespace Attendance.Domain.Interfaces
{
    public interface IRoleAdaptor
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
    }
}

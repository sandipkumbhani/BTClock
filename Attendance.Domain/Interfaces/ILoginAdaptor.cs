using Attendance.Domain.Models;

namespace Attendance.Domain.Interfaces
{
    public interface ILoginAdaptor
    {
        Task<LoginResponseData?> LoginAsync(User model);
    }
}

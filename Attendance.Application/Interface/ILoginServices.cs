using Attendance.Domain.Models;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface ILoginServices
    {
        Task<LoginResponseData?> Login(User model);
    }
}

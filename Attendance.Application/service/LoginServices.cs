using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using System.Threading.Tasks;

namespace Attendance.Application.Service
{
    public class LoginServices : ILoginServices
    {
        private readonly ILoginAdaptor _loginAdaptor;

        public LoginServices(ILoginAdaptor loginAdaptor)
        {
            _loginAdaptor = loginAdaptor;
        }

        public async Task<LoginResponseData?> Login(User model)
        {
            return await _loginAdaptor.LoginAsync(model);
        }
    }
}

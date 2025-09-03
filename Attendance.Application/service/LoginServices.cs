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
    public class LoginServices : ILoginServices
    {
        private readonly ILoginAdaptor _loginAdaptor;

        public LoginServices(ILoginAdaptor loginAdaptor)
        {
            _loginAdaptor = loginAdaptor;
        }
        public async Task<string> Login(Employee model)
        {
            return await _loginAdaptor.PostApiDataAsync(model);
        }
    }
}

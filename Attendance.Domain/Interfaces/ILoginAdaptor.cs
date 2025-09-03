using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface ILoginAdaptor
    {
        Task<string> PostApiDataAsync(Employee model);
    }
}

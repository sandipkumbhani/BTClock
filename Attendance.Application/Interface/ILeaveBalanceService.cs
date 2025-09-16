using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface ILeaveBalanceService
    {
        Task<List<LeaveBalanceDto>> GetAllLeaveBalances();
        Task<IEnumerable<LeaveBalanceDto>> GetLeaveBalanceByEmployeeId(int employeeId);
    }
}

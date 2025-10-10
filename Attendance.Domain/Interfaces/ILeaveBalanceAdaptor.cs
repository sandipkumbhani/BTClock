using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface ILeaveBalanceAdaptor
    {
        Task<List<LeaveBalanceDto>> GetAllLeaveBalances();
        Task<IEnumerable<LeaveBalanceDto>> GetLeaveBalanceByUserId(int userId);
        Task<LeaveBalanceDto> UpsertLeaveBalance(LeaveBalanceDto leaveBalance);
	}
}

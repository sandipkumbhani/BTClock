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
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly ILeaveBalanceAdaptor _leaveBalanceAdaptor;

        public LeaveBalanceService(ILeaveBalanceAdaptor leaveBalanceAdaptor)
        {
            _leaveBalanceAdaptor = leaveBalanceAdaptor;
        }
        public async Task<List<LeaveBalanceDto>> GetAllLeaveBalances()
        {
            return await _leaveBalanceAdaptor.GetAllLeaveBalances();
        }
        public async Task<IEnumerable<LeaveBalanceDto>> GetLeaveBalanceByEmployeeId(int employeeId)
        {
            return await _leaveBalanceAdaptor.GetLeaveBalanceByEmployeeId(employeeId);
        }
    }
}

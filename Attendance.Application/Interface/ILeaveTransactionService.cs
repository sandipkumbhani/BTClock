using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface ILeaveTransactionService
    {
        Task<IEnumerable<LeaveTransactionDto>> GetLeaveTransactionsByUserId(int userId);
        Task<LeaveTransactionDto> GetLeaveTransactionById(int id);
        Task<List<LeaveTransactionDto>> GetAllLeaveTransactions();
        Task<string> AddLeaveTransaction(LeaveTransactionDto leaveTransaction);
        Task<string> UpdateLeaveTransaction(LeaveTransactionDto leaveTransaction, int id);

        Task<List<LeaveTransactionDto>> UpdateStatus(List<int?> leaveTransactionIds);
        Task<int> DeleteLeaveTransaction(int id);
    }
}

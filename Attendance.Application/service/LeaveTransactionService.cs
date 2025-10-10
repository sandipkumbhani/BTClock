using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;

namespace Attendance.Application.service
{
    public class LeaveTransactionService : ILeaveTransactionService
    {
        private readonly ILeaveTransactionAdaptor _leaveTransactionAdaptor;

        public LeaveTransactionService(ILeaveTransactionAdaptor leaveTransactionAdaptor)
        {
            _leaveTransactionAdaptor = leaveTransactionAdaptor;
        }
        public async Task<IEnumerable<LeaveTransactionDto>> GetLeaveTransactionsByUserId(int userId)
        {
            return await _leaveTransactionAdaptor.GetLeaveTransactionsByUserId(userId);
        }
        public async Task<List<LeaveTransactionDto>> GetAllLeaveTransactions()
        {
            return await _leaveTransactionAdaptor.GetAllLeaveTransactions();
        }
        public async Task<LeaveTransactionDto> GetLeaveTransactionById(int id)
        {
            return await _leaveTransactionAdaptor.GetLeaveTransactionById(id);
        }
        public async Task<string> AddLeaveTransaction(LeaveTransactionDto leaveTransaction)
        {
            return await _leaveTransactionAdaptor.AddLeaveTransaction(leaveTransaction);
        }
        public async Task<string> UpdateLeaveTransaction(LeaveTransactionDto leaveTransaction, int id)
        {
            return await _leaveTransactionAdaptor.UpdateLeaveTransaction(leaveTransaction, id);
        }
        public async Task<int> DeleteLeaveTransaction(int id)
        {
            return await _leaveTransactionAdaptor.DeleteLeaveTransaction(id);
        }
        public async Task<List<LeaveTransactionDto>> UpdateStatus(List<int?> leaveTransactionIds)
        {
            return await _leaveTransactionAdaptor.UpdateStatusAsync(leaveTransactionIds);
        }
    }
}

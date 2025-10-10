using Attendance.Domain.Models;

namespace Attendance.Domain.Interfaces
{
    public interface ILeaveTransactionAdaptor
    {
        Task<IEnumerable<LeaveTransactionDto>> GetLeaveTransactionsByUserId(int userId);
        Task<LeaveTransactionDto> GetLeaveTransactionById(int id);
        Task<List<LeaveTransactionDto>> GetAllLeaveTransactions();
        Task<string> AddLeaveTransaction(LeaveTransactionDto leaveTransaction);
        Task<string> UpdateLeaveTransaction(LeaveTransactionDto leaveTransaction, int id);
        Task<List<LeaveTransactionDto>> UpdateStatusAsync(List<int?> leaveTransactionIds);

        Task<int> DeleteLeaveTransaction(int id);
    }
}

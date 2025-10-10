using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class LeaveTransaction
    {
        public LeaveTransaction()
        {
            leaveTransactionDto = new List<LeaveTransactionDto>();
        }

        public List<LeaveTransactionDto> leaveTransactionDto { get; set; }
    }

    public class LeaveTransactionDto
    {
        public int LeaveTransactionId { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("LeaveMasterId")]
        public int LeaveMasterId { get; set; }
        public LeaveMaster? LeaveMaster { get; set; }
        public bool IsPaid { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int TotalDays { get; set; }
        public string? Reason { get; set; }
        public DateTime AppliedOn { get; set; }
        public int AppliedBy { get; set; }
        public DateTime? Updatedat { get; set; }
        public int? Updatedby { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int? ApprovedBy { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public LeaveStatus? LeaveStatus { get; set; }
        public bool Ishalfday { get; set; }
        public string? AddFile { get; set; }
    }
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}

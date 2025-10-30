using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

		public int CompanyId { get; set; }

		public int UserId { get; set; }

		public int LeaveMasterId { get; set; }

		public bool IsPaid { get; set; } = true;

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public int TotalDays { get; set; }

		public string Reason { get; set; }

		public LeaveStatus LeaveStatus { get; set; } = LeaveStatus.Pending;
		public DateTime? ApprovedAt { get; set; }

		public int? ApprovedBy { get; set; }

		public bool IsHalfDay { get; set; } = false;

		public string? AddFile { get; set; }

		public bool IsActive { get; set; } = true;

		public DateTime? CreatedAt { get; set; }

		public int? CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public int? UpdatedBy { get; set; }
	}
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}

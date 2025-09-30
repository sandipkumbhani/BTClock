using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class LeaveBalance
    {
        public LeaveBalance()
        {
            leaveBalanceDto = new List<LeaveBalanceDto>();
        }

        public List<LeaveBalanceDto> leaveBalanceDto { get; set; }
    }

    public class LeaveBalanceDto
    {
        public int LeaveBalanceId { get; set; }
        [ForeignKey("EmployeeId")]

        public int EmployeeId { get; set; }
        public EmployeeDto? Employee { get; set; }
        [ForeignKey("LeaveMasterId")]
        public int? LeaveMasterId { get; set; }
        public LeaveMasterDto? LeaveMaster { get; set; }
        public double AssignedLeaves { get; set; }
        public double UsedLeaves { get; set; }
        public double RemainingLeaves { get; set; }
        public double? ExtraLeaves { get; set; }
	}
}

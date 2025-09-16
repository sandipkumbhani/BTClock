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
        public Employee? Employee { get; set; }
        [ForeignKey("LeaveMasterId")]
        public int LeaveMasterId { get; set; }
        public LeaveMaster? LeaveMaster { get; set; }
        public int AssignedLeaves { get; set; }
        public int UsedLeaves { get; set; }
        public int RemainingLeaves { get; set; }
    }
}

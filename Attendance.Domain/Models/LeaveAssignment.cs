using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class LeaveAssignment
    {
        public LeaveAssignment()
        {
            leaveAssignmentDto = new List<LeaveAssignmentDto>();
        }

        public List<LeaveAssignmentDto> leaveAssignmentDto { get; set; }
    }

    public class LeaveAssignmentDto
    {
        public int LeaveAssignmentId { get; set; }
        [ForeignKey("leavemasterId")]
        public int leavemasterId { get; set; }
        public LeaveMaster? LeaveMaster { get; set; }
        public int TotalAllocatedLeaves { get; set; }
        public int PaidAllocatedLeaves { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

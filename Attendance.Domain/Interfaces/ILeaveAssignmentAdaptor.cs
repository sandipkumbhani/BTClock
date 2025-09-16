using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface ILeaveAssignmentAdaptor
    {
        Task<List<LeaveAssignmentDto>> GetAllLeaveAssignments();
        Task<LeaveAssignmentDto> GetLeaveAssignmentById(int id);
        Task<string> AddLeaveAssignment(LeaveAssignmentDto leaveAssignment);
        Task<string> UpdateLeaveAssignment(LeaveAssignmentDto leaveAssignment, int id);
        Task<int> DeleteLeaveAssignment(int id);


    }
}

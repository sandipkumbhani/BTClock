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
    public class LeaveAssignmentService : ILeaveAssignmentService
    {
        private readonly ILeaveAssignmentAdaptor _leaveAssignmentAdaptor;

        public LeaveAssignmentService(ILeaveAssignmentAdaptor leaveAssignmentAdaptor)
        {
            _leaveAssignmentAdaptor = leaveAssignmentAdaptor;
        }
        public async Task<List<LeaveAssignmentDto>> GetAllLeaveAssignments()
        {
            return await _leaveAssignmentAdaptor.GetAllLeaveAssignments();
        }
        public async Task<LeaveAssignmentDto> GetLeaveAssignmentById(int id)
        {
            return await _leaveAssignmentAdaptor.GetLeaveAssignmentById(id);
        }
        public async Task<string> AddLeaveAssignment(LeaveAssignmentDto leaveAssignment)
        {
            return await _leaveAssignmentAdaptor.AddLeaveAssignment(leaveAssignment);
        }
        public async Task<string> UpdateLeaveAssignment(LeaveAssignmentDto leaveAssignment, int id)
        {
            return await _leaveAssignmentAdaptor.UpdateLeaveAssignment(leaveAssignment, id);
        }
        public async Task<int> DeleteLeaveAssignment(int id)
        {
            return await _leaveAssignmentAdaptor.DeleteLeaveAssignment(id);
        }
    }
}

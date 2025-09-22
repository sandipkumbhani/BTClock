using Attendance.Application.Interface;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    public class LeaveApprovalController : BaseAdminController
    {

        private readonly ILeaveTransactionService _leaveTransactionService;
        private readonly ILeaveMasterService _leaveMasterService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuMasterService _menuService;

        public LeaveApprovalController(ILeaveTransactionService leaveTransactionService, ILeaveMasterService leaveMasterService, IUserMenuMappingService userMenuMappingService, IMenuMasterService menuService) : base(menuService, userMenuMappingService)
        {
            _leaveTransactionService = leaveTransactionService;
            _leaveMasterService = leaveMasterService;
            _userMenuMappingService = userMenuMappingService;
            _menuService = menuService;
        }

        public IActionResult LeaveApproval()
        {
            return View();
        }
        public async Task<IActionResult> LeaveApprovalDetails()
        {
            var leavesList = await _leaveTransactionService.GetAllLeaveTransactions();
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            var employees = await _userMenuMappingService.GetAllEmployees();
            var masters = leaveMasters.Select(e => new
            {
                id = e.LeaveMasterId,
                name = e.LeaveType
            }).ToList();
            var users = employees.Select(e => new
            {
                id = e.EmployeeId,
                name = e.Name
            }).ToList();
            return Json(new { result = "success", data = leavesList, users, masters });
        }
        
    }
}

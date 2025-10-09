using Attendance.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    [Authorize]
    public class LeaveApprovalController : BaseAdminController
    {

        private readonly ILeaveTransactionService _leaveTransactionService;
        private readonly ILeaveMasterService _leaveMasterService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuMasterService _menuService;
        private readonly IMenuItemService _menuItemService;


        public LeaveApprovalController(ILeaveTransactionService leaveTransactionService, ILeaveMasterService leaveMasterService, IUserMenuMappingService userMenuMappingService, IMenuMasterService menuService, IMenuItemService menuItemService) : base(menuService, userMenuMappingService,menuItemService)
        {
            _leaveTransactionService = leaveTransactionService;
            _leaveMasterService = leaveMasterService;
            _userMenuMappingService = userMenuMappingService;
            _menuService = menuService;
            _menuItemService = menuItemService;
        }

        public IActionResult LeaveApproval()
        {
            return View();
        }
        public async Task<IActionResult> LeaveApprovalDetails()
        {
            var leavesList = await _leaveTransactionService.GetAllLeaveTransactions();
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            var employees = await _userMenuMappingService.GetAllUser();

            var masters = leaveMasters.Select(e => new
            {
                id = e.LeaveMasterId,
                name = e.LeaveType
            }).ToList();

            var users = employees.Select(e => new
            {
                id = e.UserId,
                name = e.Name
            }).ToList();

            var mappedLeaves = leavesList.Select(leave => new
            {
                leave.LeaveTransactionId,
                leave.EmployeeId,
                leave.LeaveMasterId,
                leave.StartDate,
                leave.EndDate,
                TotalDays = leave.Ishalfday ? 0.5 : leave.TotalDays, 
                leave.Reason,
                leave.Ishalfday,
                leave.AppliedOn,
                leave.Updatedat,
                leave.Updatedby,
                leave.ApprovedAt,
                leave.ApprovedBy,
                leave.LeaveStatus,
                leave.AddFile
            }).ToList();

            return Json(new { result = "success", data = mappedLeaves, users, masters });
        }

    }
}

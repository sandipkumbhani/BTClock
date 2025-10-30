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
        private readonly IUserService _userService;

        public LeaveApprovalController(ILeaveTransactionService leaveTransactionService, ILeaveMasterService leaveMasterService, IUserMenuMappingService userMenuMappingService, IMenuMasterService menuService, IMenuItemService menuItemService,IUserService userService) : base(menuService, userMenuMappingService,menuItemService)
        {
            _leaveTransactionService = leaveTransactionService;
            _leaveMasterService = leaveMasterService;
            _userMenuMappingService = userMenuMappingService;
            _menuService = menuService;
            _menuItemService = menuItemService;
      			_userService = userService;
		}

        public IActionResult LeaveApproval()
        {
            return View();
        }
        public async Task<IActionResult> LeaveApprovalDetails()
        {
            var leavesList = await _leaveTransactionService.GetAllLeaveTransactions();
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            var user = await _userService.GetAllUser();

			var masters = leaveMasters.Select(e => new
            {
                id = e.LeaveMasterId,
                name = e.Name
            }).ToList();

            var users = user.Select(e => new
            {
                id = e.UserId,
                name = e.Name
            }).ToList();

            var mappedLeaves = leavesList.Select(leave => new
            {
                leave.LeaveTransactionId,
                leave.UserId,
                leave.LeaveMasterId,
                leave.StartDate,
                leave.EndDate,
                TotalDays = leave.IsHalfDay ? 0.5 : leave.TotalDays, 
                leave.Reason,
                leave.IsHalfDay,
                leave.CreatedAt,
                leave.UpdatedAt,
                leave.UpdatedBy,
                leave.ApprovedAt,
                leave.ApprovedBy,
                leave.LeaveStatus,
                leave.AddFile
            }).ToList();

            return Json(new { result = "success", data = mappedLeaves, users, masters });
        }

    }
}

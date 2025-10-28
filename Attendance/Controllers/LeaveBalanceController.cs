using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Attendance.Controllers
{
    [Authorize]
    public class LeaveBalanceController : BaseLeaveController
    {
        private readonly ILogger<LeaveBalanceController> _logger;
        private readonly IConfiguration _configuration;
        private readonly GlobalClass _globalClass;
        private readonly ApplicationURL applicationURL;
        private readonly ILeaveBalanceService _leaveBalanceService;
        private readonly ILeaveMasterService _leaveMasterService;
        private readonly ILeaveAssignmentService _leaveAssignmentService;
        private readonly ILeaveTransactionService _leaveTransactionService;
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuItemService _menuItemService;
        private readonly IUserService _userService;


        public LeaveBalanceController(
        ILogger<LeaveBalanceController> logger,
        IConfiguration configuration,
        GlobalClass globalClass,
        ILeaveBalanceService leaveBalanceService,
        ILeaveMasterService leaveMaster,
        ILeaveAssignmentService leaveAssignment,
        ILeaveTransactionService leaveTransaction,
        IMenuMasterService menuService,
        IUserMenuMappingService userMenuMappingService,

        IMenuItemService menuItemService,
        IUserService userService) : base(menuService, userMenuMappingService, menuItemService)
        {
            _logger = logger;
            _configuration = configuration;
            _globalClass = globalClass;
            applicationURL = new ApplicationURL(configuration);
            _leaveBalanceService = leaveBalanceService;
            _leaveMasterService = leaveMaster;
            _leaveAssignmentService = leaveAssignment;
            _leaveTransactionService = leaveTransaction;
            _menuService = menuService;
            _userMenuMappingService = userMenuMappingService;
            _menuItemService = menuItemService;
            _userService = userService;
        }


        public async Task<IActionResult> LeaveBalance()
        {
            var users = await _userService.GetAllUser();
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            var leaveAssignments = await _leaveAssignmentService.GetAllLeaveAssignments();
            var leaveTransactions = await _leaveTransactionService.GetAllLeaveTransactions();

            var balances = new List<LeaveBalanceDto>();

            foreach (var user in users)
            {
                foreach (var master in leaveMasters)
                {
                    var allocated = leaveAssignments
                        .FirstOrDefault(x => x.leavemasterId == master.LeaveMasterId);

                    var allocatedLeaves = allocated?.TotalAllocatedLeaves ?? 0;

                    var usedLeaves = leaveTransactions
                        .Where(x => x.UserId == user.UserId && x.LeaveMasterId == master.LeaveMasterId && x.LeaveStatus == LeaveStatus.Approved)
                        .Sum(x => x.Ishalfday ? 0.5 : x.TotalDays);
                    var balance = allocatedLeaves - usedLeaves;
                    if (balance < 0) balance = 0;

                    var extraLeaves = usedLeaves > allocatedLeaves
                        ? (usedLeaves - allocatedLeaves)
                        : 0;
                    balances.Add(new LeaveBalanceDto
                    {
                        UserId = user.UserId,
                        LeaveMasterId = master.LeaveMasterId,
                        AssignedLeaves = allocatedLeaves,
                        UsedLeaves = usedLeaves,
                        RemainingLeaves = balance,
                        ExtraLeaves = extraLeaves
                    });
                }
            }

            foreach (var bal in balances)
            {
                await _leaveBalanceService.UpsertLeaveBalance(bal);
            }

            var leavebalance = await _leaveBalanceService.GetAllLeaveBalances();
            ViewBag.LeaveBalances = balances;
            ViewBag.Users = users;
            ViewBag.LeaveMasters = leaveMasters;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetallLeavebalance()
        {
            var currentUserId = UserUtility.GetUserId(HttpContext);
            var leaveList = await _leaveBalanceService.GetAllLeaveBalances();
            var userLeaveList = leaveList.Where(l => l.UserId == Convert.ToInt32(currentUserId)).ToList();

            return Json(userLeaveList);
        }
    }
}

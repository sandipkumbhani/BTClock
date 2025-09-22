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
		private readonly IEmployeeService _employeeService;
		private readonly ILeaveMasterService _leaveMasterService;
		private readonly ILeaveAssignmentService _leaveAssignmentService;
		private readonly ILeaveTransactionService _leaveTransactionService;
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;

        public LeaveBalanceController(
        ILogger<LeaveBalanceController> logger,
        IConfiguration configuration,
        GlobalClass globalClass,
        ILeaveBalanceService leaveBalanceService,
        IEmployeeService employeeService,
        ILeaveMasterService leaveMaster,
        ILeaveAssignmentService leaveAssignment,
        ILeaveTransactionService leaveTransaction,
        IMenuMasterService menuService,
        IUserMenuMappingService userMenuMappingService) : base(menuService, userMenuMappingService)
        {
            _logger = logger;
            _configuration = configuration;
            _globalClass = globalClass;
            applicationURL = new ApplicationURL(configuration);
            _leaveBalanceService = leaveBalanceService;
            _employeeService = employeeService;
            _leaveMasterService = leaveMaster;
            _leaveAssignmentService = leaveAssignment;
            _leaveTransactionService = leaveTransaction;
            _menuService = menuService;
            _userMenuMappingService = userMenuMappingService;
        }


        public async Task<IActionResult> LeaveBalance()
		{
			var employees = await _employeeService.GetAllEmployee();
			var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
			var leaveAssignments = await _leaveAssignmentService.GetAllLeaveAssignments();
			var leaveTransactions = await _leaveTransactionService.GetAllLeaveTransactions();

			var balances = new List<LeaveBalanceDto>();

			foreach (var emp in employees)
			{
				foreach (var master in leaveMasters)
				{
					var allocated = leaveAssignments
						.FirstOrDefault(x => x.DepartmentId == emp.DepartmentId && x.leavemasterId == master.LeaveMasterId);

					var allocatedLeaves = allocated?.TotalAllocatedLeaves ?? 0;

					var usedLeaves = leaveTransactions
						.Where(x => x.EmployeeId == emp.EmployeeId && x.LeaveMasterId == master.LeaveMasterId && x.LeaveStatus == LeaveStatus.Approved)
						.Sum(x => x.TotalDays);

					var balance = allocatedLeaves - usedLeaves;
					if (balance < 0) balance = 0;

					var extraLeaves = usedLeaves > allocatedLeaves
						? (usedLeaves - allocatedLeaves)
						: 0;
					balances.Add(new LeaveBalanceDto
					{
						EmployeeId = emp.EmployeeId,
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
			ViewBag.Employees = employees;
			ViewBag.LeaveMasters = leaveMasters;
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> GetallLeavebalance()
		{
			var currentUserId = UserUtility.GetUserId(HttpContext);
			var leaveList = await _leaveBalanceService.GetAllLeaveBalances();
			var employeeLeaveList = leaveList.Where(l => l.EmployeeId == currentUserId).ToList();

			return Json(employeeLeaveList);
		}
	}
}

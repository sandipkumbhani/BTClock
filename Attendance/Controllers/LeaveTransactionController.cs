using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
	[Authorize]
	public class LeaveTransactionController : BaseLeaveController
	{
		private readonly ILogger<LeaveTransactionController> _logger;
		private readonly IConfiguration _configuration;
		private readonly GlobalClass _globalClass;
		private readonly ApplicationURL _applicationURL;

		private readonly ILeaveTransactionService _leaveTransactionService;
		private readonly ILeaveMasterService _leaveMasterService;
		private readonly IUserService _userService;
		private readonly IMenuMasterService _menuService;
		private readonly IUserMenuMappingService _userMenuMappingService;
		private readonly IMenuItemService _menuItemService;

		public LeaveTransactionController(
			ILogger<LeaveTransactionController> logger,
			IConfiguration configuration,
			GlobalClass globalClass,
			ILeaveTransactionService leaveTransactionService,
			ILeaveMasterService leaveMasterService,
			IUserService userService,
			IMenuMasterService menuService,
			IUserMenuMappingService userMenuMappingService,
			IMenuItemService menuItemService
		) : base(menuService, userMenuMappingService, menuItemService)
		{
			_logger = logger;
			_configuration = configuration;
			_globalClass = globalClass;
			_applicationURL = new ApplicationURL(configuration);
			_leaveTransactionService = leaveTransactionService;
			_leaveMasterService = leaveMasterService;
			_userService = userService;
			_menuService = menuService;
			_userMenuMappingService = userMenuMappingService;
			_menuItemService = menuItemService;
		}

		public async Task<IActionResult> LeaveTransaction()
		{
			try
			{
				var leaveTransactions = await _leaveTransactionService.GetAllLeaveTransactions();
				var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
				var users = await _userService.GetAllUser();

				ViewBag.LeaveTransactions = leaveTransactions;
				ViewBag.LeaveMasters = leaveMasters;
				ViewBag.Users = users;
				ViewBag.appUrl = _applicationURL.url;

				return View();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading Leave Transaction page");
				ViewBag.errormsg = "An error occurred while loading leave transactions.";
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> LeaveTransaction(LeaveTransactionDto leaveTransactionDto, IFormFile AddFile)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.errormsg = "Please fill all required fields.";
				return await ReloadViewData(leaveTransactionDto);
			}

			try
			{
				var userId = UserUtility.GetUserId(HttpContext);
				leaveTransactionDto.UserId = int.Parse(userId);
				leaveTransactionDto.CompanyId=2;
				leaveTransactionDto.LeaveStatus = LeaveStatus.Pending;
				leaveTransactionDto.CreatedAt = DateTime.Now;
				leaveTransactionDto.CreatedBy = int.Parse(userId);

				if (AddFile != null && AddFile.Length > 0)
				{
					var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "leavefiles");
					if (!Directory.Exists(uploadsFolder))
						Directory.CreateDirectory(uploadsFolder);

					var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(AddFile.FileName)}";
					var filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await AddFile.CopyToAsync(stream);
					}

					leaveTransactionDto.AddFile = filePath;
				}
				else
				{
					_logger.LogWarning("⚠️ No file was received in LeaveTransaction");
				}

				await _leaveTransactionService.AddLeaveTransaction(leaveTransactionDto);

				TempData["msg"] = "Leave request submitted successfully!";
				return RedirectToAction("LeaveTransactionView");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding Leave Transaction");
				ViewBag.errormsg = "An error occurred while submitting leave request.";
				return await ReloadViewData(leaveTransactionDto);
			}
		}


		public async Task<IActionResult> LeaveTransactionView()
		{
			var leaveTransactions = await _leaveTransactionService.GetAllLeaveTransactions();
			ViewBag.LeaveTransactions = leaveTransactions;
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> LeaveTransactionViewDetails()
		{
			var currentUserId = UserUtility.GetUserId(HttpContext);
			var leavesList = await _leaveTransactionService.GetAllLeaveTransactions();
			var userLeaves = leavesList.Where(l => l.UserId == Convert.ToInt32(currentUserId)).ToList();

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

			return Json(new { result = "success", data = userLeaves, users, masters });
		}

		[HttpPost]
		public async Task<IActionResult> UpdateLeaveStatus(int id, LeaveStatus newStatus)
		{
			try
			{
				var leave = await _leaveTransactionService.GetLeaveTransactionById(id);
				if (leave == null)
					return Json(new { result = "error", message = "Leave transaction not found." });

				leave.LeaveStatus = newStatus; 
				leave.UpdatedAt = DateTime.Now;

				await _leaveTransactionService.UpdateLeaveTransaction(leave, id);

				return Json(new { result = "success", message = $"Leave status updated to {newStatus}." });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating Leave Transaction status");
				return Json(new { result = "error", message = "Error updating leave status." });
			}
		}

		[HttpPost]
		public async Task<IActionResult> DeleteLeaveTransaction(int id)
		{
			try
			{
				await _leaveTransactionService.DeleteLeaveTransaction(id);
				return Json(new { result = "success", message = "Leave transaction deleted successfully." });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting Leave Transaction");
				return Json(new { result = "error", message = "An error occurred while deleting the record." });
			}
		}

		private async Task<IActionResult> ReloadViewData(LeaveTransactionDto dto)
		{
			ViewBag.LeaveMasters = await _leaveMasterService.GetAllLeaveMasters();
			ViewBag.Users = await _userService.GetAllUser();
			ViewBag.LeaveTransactions = await _leaveTransactionService.GetAllLeaveTransactions();
			return View("LeaveTransaction", dto);
		}
	}
}

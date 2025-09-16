using Attendance.Application.Interface;
using Attendance.Application.service;
using Attendance.Domain.Helper;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace Attendance.Controllers
{
    public class LeaveTransactionController : Controller
    {
        private readonly ILogger<UserMenuMappingController> _logger;
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly ILeaveTransactionService _leaveTransactionService;
        private readonly ILeaveMasterService _leaveMasterService;
        private readonly IUserMenuMappingService _userMenuMappingService;

        public LeaveTransactionController(ILogger<UserMenuMappingController> logger, IConfiguration configuration, GlobalClass globalClass, ILeaveTransactionService leaveTransactionService, ILeaveMasterService leaveMasterService, IUserMenuMappingService userMenuMappingService)
        {
            _logger = logger;
            _configuration = configuration;
            applicationURL = new ApplicationURL(configuration);
            _globalClass = globalClass;
            _leaveTransactionService = leaveTransactionService;
            _leaveMasterService = leaveMasterService;
            _userMenuMappingService = userMenuMappingService;
        }

        public async Task<IActionResult> LeaveTransaction()
        {
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaveMasters = leaveMasters;
            var leaves = await _leaveTransactionService.GetAllLeaveTransactions();
            ViewBag.leaves = leaves;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LeaveTransaction(LeaveTransactionDto leaveTransaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingLeaves = await _leaveTransactionService.GetAllLeaveTransactions();
                    var leaveExists = existingLeaves.Any(l => l.EmployeeId == leaveTransaction.EmployeeId && l.LeaveMasterId == leaveTransaction.LeaveMasterId && l.StartDate == leaveTransaction.StartDate && l.EndDate == leaveTransaction.EndDate);
                    if (leaveExists)
                    {
                        ViewBag.errormsg = "Leave Transaction Already Exists";
                    }
                    else
                    {
                        var userid = UserUtility.GetUserId(HttpContext);
                        await _leaveTransactionService.AddLeaveTransaction(new LeaveTransactionDto
                        {
                            LeaveMasterId = leaveTransaction.LeaveMasterId,
                            IsPaid = leaveTransaction.IsPaid,
                            StartDate = leaveTransaction.StartDate,
                            EndDate = leaveTransaction.EndDate,
                            TotalDays = leaveTransaction.TotalDays,
                            Reason = leaveTransaction.Reason,
                            Ishalfday = leaveTransaction.Ishalfday,
                            AppliedBy = userid,
                            AppliedOn = DateTime.Now,
                            LeaveStatus = LeaveStatus.Pending,
                            EmployeeId = userid
                        });
                        ViewBag.appUrl = applicationURL.url;
                        ViewBag.msg = "Leave saved successfully!";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in LeaveTransaction POST");
                    ViewBag.errormsg = "An error occurred while processing your request.";
                }
            }
            else
            {
                ViewBag.errormsg = "Fill The Form.";
            }
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaveMasters = leaveMasters;
            var leaves = await _leaveTransactionService.GetAllLeaveTransactions();
            ViewBag.leaves = leaves;
            return View(leaveTransaction);
        }
        public async Task<IActionResult> LeaveTransactionView()
        {
            return View();
        }
        public async Task<IActionResult> LeaveTransactionViewDetails()
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
        public async Task<IActionResult> UpdateLeaveTransaction(int id)
        {
            var leaveTransaction = await _leaveTransactionService.GetLeaveTransactionById(id);
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaveMasters = leaveMasters;
            var leaves = await _leaveTransactionService.GetAllLeaveTransactions();
            ViewBag.leaves = leaves;
            return View(leaveTransaction);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLeaveTransaction(LeaveTransactionDto leaveTransaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingLeaves = await _leaveTransactionService.GetAllLeaveTransactions();
                    var leaveExists = existingLeaves.Any(l => l.LeaveMasterId == leaveTransaction.LeaveMasterId && l.StartDate == leaveTransaction.StartDate && l.EndDate == leaveTransaction.EndDate && l.LeaveTransactionId != leaveTransaction.LeaveTransactionId);
                    if (leaveExists)
                    {
                        ViewBag.errormsg = "Leave Transaction Already Exists";
                        return View(leaveTransaction);
                    }
                    var existingLeaveTransaction = await _leaveTransactionService.GetLeaveTransactionById(leaveTransaction.LeaveTransactionId);

                    if (existingLeaveTransaction == null)
                    {
                        ViewBag.errormsg = "Leave Transaction not found.";
                        return View(leaveTransaction);
                    }
                    else
                    {
                        var userid = UserUtility.GetUserId(HttpContext);
                        var leavemodel = new LeaveTransactionDto
                        {
                            LeaveMasterId = leaveTransaction.LeaveMasterId,
                            IsPaid = leaveTransaction.IsPaid,
                            StartDate = leaveTransaction.StartDate,
                            EndDate = leaveTransaction.EndDate,
                            TotalDays = leaveTransaction.TotalDays,
                            Reason = leaveTransaction.Reason,
                            Ishalfday = leaveTransaction.Ishalfday,
                            LeaveStatus = existingLeaveTransaction.LeaveStatus,
                            EmployeeId = existingLeaveTransaction.EmployeeId
                        };
                        await _leaveTransactionService.UpdateLeaveTransaction(leavemodel, leaveTransaction.LeaveTransactionId);

                    }
                    TempData["msg"] = "Leave updated successfully!";
                    return RedirectToAction("LeaveTransactionView", "LeaveTransaction");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in UpdateLeaveTransaction POST");
                    ViewBag.errormsg = "An error occurred while processing your request.";
                }
            }
            else
            {
                ViewBag.errormsg = "Fill The Form.";
            }
            var leavetransactionDto = await _leaveTransactionService.GetLeaveTransactionById(leaveTransaction.LeaveTransactionId);
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaveMasters = leaveMasters;
            var leaves = await _leaveTransactionService.GetAllLeaveTransactions();
            ViewBag.leaves = leaves;
            return View(leavetransactionDto);

        }
        public async Task<IActionResult> DeleteLeaveTransaction(int id)
        {
            try
            {
                await _leaveTransactionService.DeleteLeaveTransaction(id);
                return RedirectToAction("LeaveTransactionView", "LeaveTransaction");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Leave Transaction");
                return Json(new { success = false, message = "Error occurred while deleting Leave Transaction." });
            }
        }
        public IActionResult LeaveApproval()
        {
            return View();
        }
        public async Task<IActionResult> UpdateLeaveTransactionStatus(int id, string status)
        {
            try
            {
                var leaveTransaction = await _leaveTransactionService.GetLeaveTransactionById(id);
                if (leaveTransaction == null)
                {
                    return Json(new { success = false, message = "Leave Transaction not found." });
                }
                if (status != "Approved" && status != "Rejected")
                {
                    return Json(new { success = false, message = "Invalid status value." });
                }
                leaveTransaction.LeaveStatus = status == "Approved" ? LeaveStatus.Approved : LeaveStatus.Rejected;
                await _leaveTransactionService.UpdateLeaveTransaction(leaveTransaction, id);
                return Json(new { success = true, message = "Leave status updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Leave Transaction status");
                return Json(new { success = false, message = "Error occurred while updating Leave status." });
            }
        }

    }
}

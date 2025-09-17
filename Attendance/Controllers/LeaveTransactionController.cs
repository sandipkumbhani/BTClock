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
                            AppliedOn = DateTime.Now,
                            AppliedBy = userid,
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
                    var leaveExists = existingLeaves.Any(l =>
                        l.LeaveMasterId == leaveTransaction.LeaveMasterId &&
                        l.StartDate == leaveTransaction.StartDate &&
                        l.EndDate == leaveTransaction.EndDate &&
                        l.LeaveTransactionId != leaveTransaction.LeaveTransactionId);


                    var existingLeaveTransaction = await _leaveTransactionService.GetLeaveTransactionById(leaveTransaction.LeaveTransactionId);
                    if (existingLeaveTransaction == null)
                    {
                        ViewBag.errormsg = "Leave Transaction not found.";
                        return View(leaveTransaction);
                    }

                    var newStatus = existingLeaveTransaction.LeaveStatus == LeaveStatus.Rejected
                        ? LeaveStatus.Pending
                        : existingLeaveTransaction.LeaveStatus;
                    var currentUserId = UserUtility.GetUserId(HttpContext);

                    var updatedLeaveTransaction = new LeaveTransactionDto
                    {
                        LeaveMasterId = leaveTransaction.LeaveMasterId,
                        IsPaid = leaveTransaction.IsPaid,
                        StartDate = leaveTransaction.StartDate,
                        EndDate = leaveTransaction.EndDate,
                        TotalDays = leaveTransaction.TotalDays,
                        Reason = leaveTransaction.Reason,
                        Ishalfday = leaveTransaction.Ishalfday,
                        Updatedat = DateTime.Now,
                        Updatedby = Convert.ToInt32(currentUserId),
                        EmployeeId = existingLeaveTransaction.EmployeeId,
                        LeaveStatus = newStatus
                    };

                    await _leaveTransactionService.UpdateLeaveTransaction(updatedLeaveTransaction, leaveTransaction.LeaveTransactionId);

                    TempData["msg"] = newStatus == LeaveStatus.Pending
                        ? "Leave updated and status reset to pending."
                        : "Leave updated successfully!";

                    // Redirect to view page
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

            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaveMasters = leaveMasters;
            var leaves = await _leaveTransactionService.GetAllLeaveTransactions();
            ViewBag.leaves = leaves;
            return View(leaveTransaction);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLeaveTransaction(int id)
        {
            try
            {
                await _leaveTransactionService.DeleteLeaveTransaction(id);

                TempData["msg"] = "Leave Transaction deleted successfully.";
                return RedirectToAction("LeaveTransactionView", "LeaveTransaction");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Leave Transaction");
                TempData["ErrorMsg"] = "Error occurred while deleting Leave Transaction.";
                return RedirectToAction("LeaveTransactionView", "LeaveTransaction");
            }
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
        [HttpPost]
        public async Task<IActionResult> UpdateLeaveTransactionStatus(List<int?> leaveTransactionIds, string status)
        {
            if (leaveTransactionIds == null || !leaveTransactionIds.Any())
            {
                return BadRequest(new { success = false, message = "No leave IDs provided." });
            }

            if (status != "Approved" && status != "Rejected")
            {
                return BadRequest(new { success = false, message = "Invalid status value." });
            }

            var approve = await _leaveTransactionService.GetLeaveTransactionById(leaveTransactionIds.First().Value);
            if (approve == null)
            {
                return BadRequest(new { success = false, message = "First leave transaction not found." });
            }

            var currentUserId = UserUtility.GetUserId(HttpContext);

            var updatedLeaveStatus = new LeaveTransactionDto
            {
                ApprovedAt = DateTime.Now,
                ApprovedBy = Convert.ToInt32(currentUserId)
            };

            foreach (var id in leaveTransactionIds)
            {
                var leave = await _leaveTransactionService.GetLeaveTransactionById(id.Value);
                if (leave != null)
                {
                    leave.LeaveStatus = status == "Approved" ? LeaveStatus.Approved : LeaveStatus.Rejected;

                    if (status == "Approved")
                    {
                        leave.ApprovedAt = updatedLeaveStatus.ApprovedAt;
                        leave.ApprovedBy = updatedLeaveStatus.ApprovedBy;
                    }

                    await _leaveTransactionService.UpdateLeaveTransaction(leave, id.Value);
                }
            }

            return Json(new { success = true, message = $"Status updated to '{status}' for selected leaves." });
        }


    }
}

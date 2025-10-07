using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Attendance.Controllers
{
    [Authorize]
    public class LeaveTransactionController : BaseLeaveController
    {
        private readonly ILogger<UserMenuMappingController> _logger;
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly ILeaveTransactionService _leaveTransactionService;
        private readonly ILeaveMasterService _leaveMasterService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuMasterService _menuService;


        public LeaveTransactionController(ILogger<UserMenuMappingController> logger, IConfiguration configuration, GlobalClass globalClass, ILeaveTransactionService leaveTransactionService, ILeaveMasterService leaveMasterService, IUserMenuMappingService userMenuMappingService, IMenuMasterService menuService) : base(menuService, userMenuMappingService)
        {
            _logger = logger;
            _configuration = configuration;
            applicationURL = new ApplicationURL(configuration);
            _globalClass = globalClass;
            _leaveTransactionService = leaveTransactionService;
            _leaveMasterService = leaveMasterService;
            _userMenuMappingService = userMenuMappingService;
            _menuService = menuService;
        }

        public async Task<IActionResult> LeaveTransaction()
        {
            if (_globalClass.Token != null)
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_globalClass.Token);
                var claims = UserUtility.addClaimstoUser(HttpContext, jwt.Claims);
                string currentPage = "Leave Transaction";
                var canAccess = UserUtility.CanAccessMenu(HttpContext, currentPage);

                if (canAccess)
                {
                    var employee = claims.Claims.FirstOrDefault(x => x.Type == "EmployeeId");
                    int EmployeeId = employee != null ? (!string.IsNullOrEmpty(employee.Value) ? Convert.ToInt32(employee.Value) : 0) : 0;
                    ViewBag.EmployeeId = EmployeeId;
                    ViewBag.appUrl = applicationURL.url;
                    var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
                    var leaves = await _leaveTransactionService.GetAllLeaveTransactions();
                    ViewBag.leaveMasters = leaveMasters;
                    ViewBag.leaves = leaves;
                    return View();
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LeaveTransaction(LeaveTransactionDto leaveTransaction, IFormFile AddFile)
        {
            if (ModelState.IsValid || leaveTransaction.AddFile == null)
            {
                try
                {
                    string filename = null;

                    if (AddFile != null && AddFile.Length > 0)
                    {
                        filename = UploadFile(AddFile);
                    }

                    var existingLeaves = await _leaveTransactionService.GetAllLeaveTransactions();
                    var leaveExists = existingLeaves.Any(l => l.EmployeeId == leaveTransaction.EmployeeId &&
                                                             l.LeaveMasterId == leaveTransaction.LeaveMasterId &&
                                                             l.StartDate == leaveTransaction.StartDate &&
                                                             l.EndDate == leaveTransaction.EndDate);

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
                            AddFile = leaveTransaction.AddFile,
                            EmployeeId = userid,
                        });
                        ViewBag.appUrl = applicationURL.url;
                        ViewBag.msg = "Leave saved successfully!";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in LeaveTransaction ");
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
            var currentUserId = UserUtility.GetUserId(HttpContext); // logged in employeeId
            var leavesList = await _leaveTransactionService.GetAllLeaveTransactions();
            var employeeLeaves = leavesList.Where(l => l.EmployeeId == currentUserId).ToList();

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

            return Json(new { result = "success", data = employeeLeaves, users, masters });
        }
        public string UploadFile(IFormFile file)
        {
            string fileName = null;
            if (file != null)
            {
                if (file.Length > 0)
                {
                    string file_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\leave_pdf\\");
                    fileName = Path.GetFileName(file.FileName);
                    var fileExtention = Path.GetExtension(fileName);
                    if (!Directory.Exists(file_path))
                    {
                        Directory.CreateDirectory(file_path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(file_path + fileName))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                    return fileName;
                }
            }
            return fileName;
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
        public async Task<IActionResult> UpdateLeaveTransaction(LeaveTransactionDto leaveTransaction, IFormFile AddFile, string ExistingFile)
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

                string fileName = ExistingFile;
                if (AddFile != null && AddFile.Length > 0)
                {
                    fileName = UploadFile(AddFile);
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
                    AppliedOn = existingLeaveTransaction.AppliedOn,
                    Updatedat = DateTime.Now,
                    Updatedby = Convert.ToInt32(currentUserId),
                    EmployeeId = existingLeaveTransaction.EmployeeId,
                    LeaveStatus = newStatus,
                    AddFile = fileName
                };


                await _leaveTransactionService.UpdateLeaveTransaction(updatedLeaveTransaction, leaveTransaction.LeaveTransactionId);

                TempData["msg"] = newStatus == LeaveStatus.Pending
                    ? "Leave updated and status reset to pending."
                    : "Leave updated successfully!";

                return RedirectToAction("LeaveTransactionView", "LeaveTransaction");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateLeaveTransaction POST");
                ViewBag.errormsg = "An error occurred while processing your request.";
            }

            ViewBag.leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaves = await _leaveTransactionService.GetAllLeaveTransactions();
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

        [HttpPost]
        public async Task<IActionResult> UpdateLeaveTransactionStatus(List<int?> leaveTransactionIds, string status, IFormFile AddFile)
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

            string uploadedFileName = null;
            if (AddFile != null && AddFile.Length > 0)
            {
                uploadedFileName = UploadFile(AddFile);
            }
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

                    var appliedOn = leave.AppliedOn;

                    leave.LeaveStatus = status == "Approved" ? LeaveStatus.Approved : LeaveStatus.Rejected;

                    if (status == "Approved")
                    {
                        leave.ApprovedAt = updatedLeaveStatus.ApprovedAt;
                        leave.ApprovedBy = updatedLeaveStatus.ApprovedBy;
                    }
                    leave.AppliedOn = appliedOn;
                    leave.AddFile = !string.IsNullOrEmpty(uploadedFileName) ? uploadedFileName : leave.AddFile;

                    await _leaveTransactionService.UpdateLeaveTransaction(leave, id.Value);
                }
            }

            return Json(new { success = true, message = $"Status updated to '{status}' for selected leaves." });
        }

    }
}

using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    [Authorize]
    public class LeaveAssignmentController : BaseAdminController
    {
        private readonly ILogger<LeaveAssignmentController> _logger;
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly ILeaveAssignmentService _leaveAssignmentService;
        private readonly ILeaveMasterService _leaveMasterService;
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuItemService _menuItemService;



        public LeaveAssignmentController(ILogger<LeaveAssignmentController> logger, IConfiguration configuration, GlobalClass globalClass,  ILeaveAssignmentService leaveAssignmentService, ILeaveMasterService leaveMasterService, IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService, IMenuItemService menuItemService) : base(menuService, userMenuMappingService, menuItemService)
        {
            _logger = logger;
            _configuration = configuration;
            _globalClass = globalClass;
            applicationURL = new ApplicationURL(configuration);
            _leaveAssignmentService = leaveAssignmentService;
            _leaveMasterService = leaveMasterService;
            _menuService = menuService;
            _userMenuMappingService = userMenuMappingService;
            _menuItemService = menuItemService;
        }

        public async Task<IActionResult> LeaveAssignment()
        {
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaveMasters = leaveMasters;
            var leaveAssignments = await _leaveAssignmentService.GetAllLeaveAssignments();
            ViewBag.leaveAssignments = leaveAssignments;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LeaveAssignment(LeaveAssignmentDto leaveAssignment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingAssignments = await _leaveAssignmentService.GetAllLeaveAssignments();
                    var assignmentExists = existingAssignments.Any(l => l.leavemasterId == leaveAssignment.leavemasterId);
                    if (assignmentExists)
                    {
                        ViewBag.errormsg = "Leave Assignment Already Exists";
                    }
                    else
                    {
                        var userid = UserUtility.GetUserId(HttpContext);
                        await _leaveAssignmentService.AddLeaveAssignment(new LeaveAssignmentDto
                        {
                            leavemasterId = leaveAssignment.leavemasterId,
                            TotalAllocatedLeaves = leaveAssignment.TotalAllocatedLeaves,
                            PaidAllocatedLeaves = leaveAssignment.PaidAllocatedLeaves,

                        });
                        ViewBag.appUrl = applicationURL.url;
                        ViewBag.msg = "Leave Assignment Added Successfully";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Leave Assignment");
                    ViewBag.errormsg = "An error occurred while processing your request.";
                }
            }
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaveMasters = leaveMasters;
            var leaveAssignments = await _leaveAssignmentService.GetAllLeaveAssignments();
            ViewBag.leaveAssignments = leaveAssignments;
            return View(leaveAssignment);
        }
        public async Task<IActionResult> LeaveAssignmentView()
        {

            return View();
        }
        public async Task<IActionResult> LeaveAssignmentViewDetails()
        {
            var leaveAssignments = await _leaveAssignmentService.GetAllLeaveAssignments();
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            var masters = leaveMasters.Select(e => new
            {
                id = e.LeaveMasterId,
                name = e.Name
            }).ToList();
            
            return Json(new { result = "success", data = leaveAssignments, masters });
        }
        public async Task<IActionResult> UpdateLeaveAssignment(int id)
        {
            var leaveAssignment = await _leaveAssignmentService.GetLeaveAssignmentById(id);
            var leaves = await _leaveAssignmentService.GetAllLeaveAssignments();
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaveMasters = leaveMasters;
            return View(leaveAssignment);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLeaveAssignment(LeaveAssignmentDto leaveAssignment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingAssignments = await _leaveAssignmentService.GetAllLeaveAssignments();
                    var assignmentExists = existingAssignments.Any(l => l.leavemasterId == leaveAssignment.leavemasterId && l.LeaveAssignmentId != leaveAssignment.LeaveAssignmentId);
                    if (assignmentExists)
                    {
                        ViewBag.errormsg = "Leave Assignment Already Exists";
                    }
                    var existingLeaveAssignment = await _leaveAssignmentService.GetLeaveAssignmentById(leaveAssignment.LeaveAssignmentId);
                    if (existingLeaveAssignment == null)
                    {
                        ViewBag.errormsg = "Leave Assignment not found.";
                    }
                    else
                    {
                        var leaveAssign = new LeaveAssignmentDto
                        {
                            leavemasterId = leaveAssignment.leavemasterId,
                            TotalAllocatedLeaves = leaveAssignment.TotalAllocatedLeaves,
                            PaidAllocatedLeaves = leaveAssignment.PaidAllocatedLeaves,
                            IsActive = existingLeaveAssignment.IsActive,
                        };
                        await _leaveAssignmentService.UpdateLeaveAssignment(leaveAssign, leaveAssignment.LeaveAssignmentId);
                       
                    }
                    TempData["msg"] = "LeaveAssign updated successfully!";
                    return RedirectToAction("LeaveAssignmentView", "LeaveAssignment");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Leave Assignment");
                    ViewBag.errormsg = "An error occurred while processing your request.";
                }
            }
            var leaveassignmentDto = await _leaveAssignmentService.GetLeaveAssignmentById(leaveAssignment.LeaveAssignmentId);
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaveMasters = leaveMasters;
            var leaves = await _leaveAssignmentService.GetAllLeaveAssignments();
            ViewBag.leaves = leaves;
            return View(leaveassignmentDto);
        }
        public async Task<IActionResult> DeleteLeaveAssignment(int id)
        {
            try
            {
                await _leaveAssignmentService.DeleteLeaveAssignment(id);
                return RedirectToAction("LeaveAssignmentView", "LeaveAssignment");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Leave Assignment");
                return Json(new { success = false, message = "An error occurred while deleting the Leave Assignment." });
            }
        }
    }
}

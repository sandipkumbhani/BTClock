using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    public class LeaveAssignmentController : BaseAdminController
    {
        private readonly ILogger<LeaveAssignmentController> _logger;
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly ILeaveAssignmentService _leaveAssignmentService;
        private readonly IDepartmentService _departmentService;
        private readonly ILeaveMasterService _leaveMasterService;
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;


        public LeaveAssignmentController(ILogger<LeaveAssignmentController> logger, IConfiguration configuration, GlobalClass globalClass, IDepartmentService departmentService, ILeaveAssignmentService leaveAssignmentService, ILeaveMasterService leaveMasterService, IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService) : base(menuService, userMenuMappingService)
        {
            _logger = logger;
            _configuration = configuration;
            _globalClass = globalClass;
            applicationURL = new ApplicationURL(configuration);
            _departmentService = departmentService;
            _leaveAssignmentService = leaveAssignmentService;
            _leaveMasterService = leaveMasterService;
            _menuService = menuService;
            _userMenuMappingService = userMenuMappingService;
        }

        public async Task<IActionResult> LeaveAssignment()
        {
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaveMasters = leaveMasters;
            var leaveAssignments = await _leaveAssignmentService.GetAllLeaveAssignments();
            ViewBag.leaveAssignments = leaveAssignments;
            var departments = await _departmentService.GetAllDepartments();
            ViewBag.departments = departments;
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
                    var assignmentExists = existingAssignments.Any(l => l.DepartmentId == leaveAssignment.DepartmentId && l.leavemasterId == leaveAssignment.leavemasterId);
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
                            DepartmentId = leaveAssignment.DepartmentId,
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
            var departments = await _departmentService.GetAllDepartments();
            ViewBag.departments = departments;
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
            var departments = await _departmentService.GetAllDepartments();
            var masters = leaveMasters.Select(e => new
            {
                id = e.LeaveMasterId,
                name = e.LeaveType
            }).ToList();
            var depts = departments.Select(d => new
            {
                id = d.Id,
                name = d.Name
            }).ToList();
            return Json(new { result = "success", data = leaveAssignments, depts, masters });
        }
        public async Task<IActionResult> UpdateLeaveAssignment(int id)
        {
            var leaveAssignment = await _leaveAssignmentService.GetLeaveAssignmentById(id);
            var leaves = await _leaveAssignmentService.GetAllLeaveAssignments();
            var leaveMasters = await _leaveMasterService.GetAllLeaveMasters();
            ViewBag.leaveMasters = leaveMasters;
            var departments = await _departmentService.GetAllDepartments();
            ViewBag.departments = departments;
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
                    var assignmentExists = existingAssignments.Any(l => l.DepartmentId == leaveAssignment.DepartmentId && l.leavemasterId == leaveAssignment.leavemasterId && l.LeaveAssignmentId != leaveAssignment.LeaveAssignmentId);
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
                            DepartmentId = leaveAssignment.DepartmentId,
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
            var departments = await _departmentService.GetAllDepartments();
            ViewBag.departments = departments;
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

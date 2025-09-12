using Attendance.Application.Interface;
using Attendance.Domain.Models;
using Attendance.UI.Domain.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
	public class MasterController : Controller
	{
		private readonly IConfiguration _configuration;
		private ApplicationURL applicationURL;
		private readonly GlobalClass _globalClass;
		private readonly IDepartmentService _departmentService;
		private readonly IDesignationService _designationService;
		private readonly ILeaveMasterService _leaveMasterService;
		private readonly IModuleMasterService _moduleMasterService;
		public MasterController(IConfiguration configuration, GlobalClass globalClass, IDepartmentService departmentService, IDesignationService designationService, ILeaveMasterService leaveMasterService, IModuleMasterService moduleMasterService)
		{
			_configuration = configuration;
			applicationURL = new ApplicationURL(configuration);
			_globalClass = globalClass;
			_departmentService = departmentService;
			_designationService = designationService;
			_leaveMasterService = leaveMasterService;
			_moduleMasterService = moduleMasterService;
		}
		public IActionResult MasterTable()
		{
			if (_globalClass.Token != null)
			{
				ViewBag.appUrl = applicationURL.url;
				return View();
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ModuleMaster(ModuleMasterDto moduleMasterDto)
		{
			try
			{

				ViewBag.appUrl = applicationURL.url;
				if (ModelState.IsValid)
				{
					var module = new ModuleMasterDto
					{
						ModuleName = moduleMasterDto.ModuleName,
					};
					var result = await _moduleMasterService.AddModuleMaster(module);
					if (result != null)
					{
						TempData["ModelSuccessMsg"] = "Module added successfully";
						TempData["OpenPanel"] = "module";
						ModelState.Clear();
					}
					else
					{
						TempData["ModelErrorMsg"] = "Module not added";
						TempData["OpenPanel"] = "module";
					}
				}
				return RedirectToAction("MasterTable");
			}
			catch (Exception ex)
			{
				TempData["ModelErrorMsg"] = "An error occurred while processing your request.";
				TempData["OpenPanel"] = "module";
				return View();
			}
		}
		[HttpPost]
		public async Task<IActionResult> Department(DepartmentDto departmentDto)
		{
			try
			{
				ViewBag.appUrl = applicationURL.url;
				if (ModelState.IsValid)
				{
					var department = new DepartmentDto
					{
						Name = departmentDto.Name,
					};

					var result = await _departmentService.AddDepartment(department);
					if (result != null)
					{
						TempData["DeptSuccessMsg"] = "Department added successfully";
						TempData["OpenPanel"] = "dept";
						ModelState.Clear();
					}
					else
					{
						TempData["DeptErrorMsg"] = "Department not added";
						TempData["OpenPanel"] = "dept";
					}
				}
				return RedirectToAction("MasterTable");
			}
			catch (Exception ex)
			{
				TempData["DeptErrorMsg"] = "An error occurred while processing your request.";
				TempData["OpenPanel"] = "dept";
				return View();
			}
		}
		[HttpPost]
		public async Task<IActionResult> Designation(DesignationDto designationDto)
		{
			try
			{
				ViewBag.appUrl = applicationURL.url;
				if (ModelState.IsValid)
				{
					var designation = new DesignationDto
					{
						DesignationName = designationDto.DesignationName ,
					};
					var result = await _designationService.AddDesignation(designation);
					if (result != null)
					{
						TempData["DesigsSuccessMsg"] = "Designation added successfully";
						TempData["OpenPanel"] = "desig";
						ModelState.Clear();
					}
					else
					{
						TempData["DesigErrorMsg"] = "Designation not added";
						TempData["OpenPanel"] = "desig";
					}
				}
				return RedirectToAction("MasterTable");
			}
			catch (Exception ex)
			{
				TempData["DesigErrorMsg"] = "An error occurred while processing your request.";
				TempData["OpenPanel"] = "desig";
				return View();
			}
		}
		[HttpPost]
		public async Task<IActionResult> Leave(LeaveMasterDto leaveMasterDto)
		{
			try
			{
				ViewBag.appUrl = applicationURL.url;
				if (ModelState.IsValid)
				{
					var leaveMaster = new LeaveMasterDto
					{
						LeaveType = leaveMasterDto.LeaveType,
					};
					var result = await _leaveMasterService.AddLeaveMaster(leaveMaster);
					if (result != null)
					{
						TempData["LeaveSuccessMsg"] = "Leave added successfully";
						TempData["OpenPanel"] = "leave";
						ModelState.Clear();
					}
					else
					{
						TempData["LeaveErrorMsg"] = "Leave not added";
						TempData["OpenPanel"] = "leave";
					}
				}
				return RedirectToAction("MasterTable");
			}
			catch (Exception ex)
			{
				TempData["LeaveErrorMsg"] = "An error occurred while processing your request.";
				TempData["OpenPanel"] = "leave";
				return View();
			}
		}
	}
}

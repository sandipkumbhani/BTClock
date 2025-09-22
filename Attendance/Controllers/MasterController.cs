using Attendance.Application.Interface;
using Attendance.Domain.Models;
using Attendance.Domain.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Attendance.Controllers
{
	[Authorize]

	public class MasterController : Controller
	{
		private readonly IConfiguration _configuration;
		private ApplicationURL applicationURL;
		private readonly GlobalClass _globalClass;
		private readonly IDepartmentService _departmentService;
		private readonly IDesignationService _designationService;
		private readonly ILeaveMasterService _leaveMasterService;
		private readonly IModuleMasterService _moduleMasterService;
		private readonly IHolidayMasterService _holidayMasterService;
		public MasterController(IConfiguration configuration, GlobalClass globalClass, IDepartmentService departmentService, IDesignationService designationService, ILeaveMasterService leaveMasterService, IModuleMasterService moduleMasterService, IHolidayMasterService holidayMasterService)
		{
			_configuration = configuration;
			applicationURL = new ApplicationURL(configuration);
			_globalClass = globalClass;
			_departmentService = departmentService;
			_designationService = designationService;
			_leaveMasterService = leaveMasterService;
			_moduleMasterService = moduleMasterService;
			_holidayMasterService = holidayMasterService;
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
				else
				{
					TempData["ModelErrorMsg"] = "Module not added";
					TempData["OpenPanel"] = "module";
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
		[HttpGet]
		public async Task<IActionResult> GetAllModule()
		{
			try
			{
				var modules = await _moduleMasterService.GetAllModuleMaster();
				return Json(modules);
			}
			catch (Exception ex)
			{
				TempData["ModelErrorMsg"] = "An error occurred while processing your request.";
				TempData["OpenPanel"] = "module";
				return View();
			}
		}
		[HttpGet]
		public async Task<IActionResult> EditModule(int id)
		{
			var module = await _moduleMasterService.GetModuleMasterById(id);
			return Json(module);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateModule(ModuleMasterDto moduleMasterDto, int id)
		{
			var result = await _moduleMasterService.UpdateModuleMaster(moduleMasterDto, id);
			if (result != null)
			{
				TempData["ModelSuccessMsg"] = "Module updated successfully";
			}
			else
			{
				TempData["ModelErrorMsg"] = "Update failed";
			}
			TempData["OpenPanel"] = "module";
			TempData["OpenView"] = "module";

			return RedirectToAction("MasterTable");
		}
		public async Task<IActionResult> DeleteModule(int id)
		{
			var result = await _moduleMasterService.DeleteModuleMaster(id);
			if (result != 0)
			{
				TempData["ModelSuccessMsg"] = "Module deleted successfully";
			}
			else
			{
				TempData["ModelErrorMsg"] = "Delete failed";
			}
			TempData["OpenPanel"] = "module";
			TempData["OpenView"] = "module";
			return RedirectToAction("MasterTable");
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
		[HttpGet]
		public async Task<IActionResult> GetAllDepartment()
		{
			var department = await _departmentService.GetAllDepartments();
			return Json(department);
		}
		[HttpGet]
		public async Task<IActionResult> EditDepartment(int id)
		{
			var department = await _departmentService.GetDepartmentById(id);
			return Json(department);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateDepartment(int id, DepartmentDto departmentDto)
		{
			//departmentDto.Id = id;
			var result = await _departmentService.UpdateDepartment(departmentDto, id);

			TempData["DeptSuccessMsg"] = result != null ? "Department updated successfully" : "Update failed";
			TempData["OpenPanel"] = "dept";
			TempData["OpenView"] = "dept";
			return RedirectToAction("MasterTable");
		}
		public async Task<IActionResult> DeleteDept(int id)
		{
			var result = await _departmentService.DeleteDepartment(id);
			if (result != 0)
			{
				TempData["DeptSuccessMsg"] = "Department deleted successfully";

			}
			else
			{
				TempData["DeptErrorMsg"] = "Delete failed";
			}
			TempData["OpenPanel"] = "dept";
			TempData["OpenView"] = "dept";
			return RedirectToAction("MasterTable");
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
						DesignationName = designationDto.DesignationName,
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
		[HttpGet]
		public async Task<IActionResult> GetAllDesignation()
		{
			var designation = await _designationService.GetAllDesignation();
			return Json(designation);
		}
		[HttpGet]
		public async Task<IActionResult> EditDesignation(int id)
		{
			var designation = await _designationService.GetDesignationById(id);
			return Json(designation);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateDesignation(int id, DesignationDto designationDto)
		{
			//designationDto.DesignationId = id;
			var result = await _designationService.UpdateDesignation(designationDto, id);
			if (result != null)
			{
				TempData["DesigSuccessMsg"] = "Designation updated successfully";
				TempData["OpenView"] = "desig";
				TempData["OpenPanel"] = "desig";
			}
			else
			{
				TempData["DesigErrorMsg"] = "Update failed";
				TempData["OpenView"] = "desig";
				TempData["OpenPanel"] = "desig";
			}
			return RedirectToAction("MasterTable");
		}
		public async Task<IActionResult> DeleteDesig(int id)
		{
			var result = await _designationService.DeleteDesignation(id);
			if (result != 0)
			{
				TempData["DesigSuccessMsg"] = "Designation deleted successfully";
				TempData["OpenView"] = "desig";
				TempData["OpenPanel"] = "desig";
			}
			else
			{
				TempData["DesigErrorMsg"] = "Delete failed";
				TempData["OpenView"] = "desig";
				TempData["OpenPanel"] = "desig";

			}
			TempData["OpenPanel"] = "desig";
			return RedirectToAction("MasterTable");
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
		[HttpGet]
		public async Task<IActionResult> GetAllLeave()
		{
			var leave = await _leaveMasterService.GetAllLeaveMasters();
			return Json(leave);
		}
		[HttpGet]
		public async Task<IActionResult> EditLeave(int id)
		{
			var leave = await _leaveMasterService.GetLeaveMasterById(id);
			return Json(leave);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateLeave(int id, LeaveMasterDto leaveMasterDto)
		{
			//leaveMasterDto.LeaveMasterId = id;
			var result = await _leaveMasterService.UpdateLeaveMaster(leaveMasterDto, id);

			TempData["LeaveSuccessMsg"] = result != null ? "Leave type updated successfully" : "Update failed";
			TempData["OpenPanel"] = "leave";
			TempData["OpenView"] = "leave";
			return RedirectToAction("MasterTable");
		}
		public async Task<IActionResult> DeleteLeave(int id)
		{
			var result = await _leaveMasterService.DeleteLeaveMaster(id);
			if (result != 0)
			{
				TempData["LeaveSuccessMsg"] = "Leave type deleted successfully";
				TempData["OpenView"] = "leave";
			}
			else
			{
				TempData["LeaveErrorMsg"] = "Delete failed";
				TempData["OpenView"] = "leave";
			}
			TempData["OpenPanel"] = "leave";
			return RedirectToAction("MasterTable");
		}
		[HttpPost]
		public async Task<IActionResult> Holiday(HolidayMasterDto holidayMasterDto)
		{
			try
			{
				ViewBag.appUrl = applicationURL.url;
				if (ModelState.IsValid)
				{
					var holidayMaster = new HolidayMasterDto
					{
						HolidayDescription = holidayMasterDto.HolidayDescription,
						HolidayDate = holidayMasterDto.HolidayDate,
						Year = holidayMasterDto.Year,
						SaveWeekend = holidayMasterDto.SaveWeekend
					};
					var result = await _holidayMasterService.AddHolidayMaster(holidayMaster);
					if (result != null)
					{
						TempData["HolidaySuccessMsg"] = "Holiday added successfully";
						TempData["OpenPanel"] = "Holiday";
						ModelState.Clear();
					}
					else
					{
						TempData["HolidayErrorMsg"] = "Holiday not added";
						TempData["OpenPanel"] = "Holiday";
					}
				}
				return RedirectToAction("MasterTable");
			}
			catch (Exception ex)
			{
				TempData["HolidayErrorMsg"] = "An error occurred while processing your request.";
				TempData["OpenPanel"] = "holiday";
				return View();
			}
		}
		[HttpPost]
		public async Task<IActionResult> UpdateHoliday(int id, HolidayMasterDto holidayMasterDto)
		{
			//holidayMasterDto.HolidayMasterId = id;
			var result = await _holidayMasterService.UpdateHolidayMaster(holidayMasterDto, id);
			if (result != null)
			{
				TempData["HolidaySuccessMsg"] = "Holiday updated successfully";
				TempData["OpenView"] = "Holiday";
				TempData["OpenPanel"] = "Holiday";
			}
			else
			{
				TempData["HolidayErrorMsg"] = "Update failed";
				TempData["OpenView"] = "Holiday";
				TempData["OpenPanel"] = "Holiday";
			}
			return RedirectToAction("MasterTable");
		}
		[HttpGet]
		public async Task<IActionResult> GetAllHoliday()
		{
			var holiday = await _holidayMasterService.GetAllHolidayMaster();
			return Json(holiday);
		}
		[HttpGet]
		public async Task<IActionResult> EditHoliday(int id)
		{
			var holiday = await _holidayMasterService.GetHolidayMasterById(id);
			return Json(holiday);
		}
		public async Task<IActionResult> DeleteHoliday(int id)
		{
			var result = await _holidayMasterService.DeleteHolidayMaster(id);
			if (result != 0)
			{
				TempData["HolidaySuccessMsg"] = "Holiday deleted successfully";
				TempData["OpenView"] = "Holiday";
			}
			else
			{
				TempData["HolidayErrorMsg"] = "Delete failed";
				TempData["OpenView"] = "Holiday";
			}
			TempData["OpenPanel"] = "Holiday";
			return RedirectToAction("MasterTable");
		}
	}
}

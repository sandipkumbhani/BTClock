using Attendance.Application.Interface;
using Attendance.Application.service;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
	[Authorize]

	public class PermissionController : Controller
	{
		private readonly IConfiguration _configuration;
		private ApplicationURL applicationURL;
		private readonly GlobalClass _globalClass;
		private readonly IDesignationService _designationService;
		private readonly IModuleMasterService _moduleMasterService;
		private readonly IPermissionService _permissionService;
		public PermissionController(IConfiguration configuration, GlobalClass globalClass, IDesignationService designationService, IPermissionService permissionService, IModuleMasterService moduleMasterService)
		{
			_configuration = configuration;
			applicationURL = new ApplicationURL(configuration);
			_globalClass = globalClass;
			_designationService = designationService;
			_permissionService = permissionService;
			_moduleMasterService = moduleMasterService;
		}
		public IActionResult Permission()
		{
			if (_globalClass.Token != null)
			{
				var designations = _designationService.GetAllDesignation().Result;
				ViewBag.Designations = designations;
				var modules = _moduleMasterService.GetAllModuleMaster().Result;
				ViewBag.Modules = modules;
			}
			else
			{
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Permission(PermissionDto permissionDto)
		{
			if (ModelState.IsValid)
			{
				var permission = new PermissionDto
				{
					DesignationId = permissionDto.DesignationId,
					ModuleMasterId = permissionDto.ModuleMasterId,
					CanAccess = permissionDto.CanAccess
				};
			}
			var result = await _permissionService.AddPermission(permissionDto);

			if (result != null)
			{
				ViewData["Message"] = "Permission added successfully.";
				ModelState.Clear();
			}
			else
			{
				ViewData["ErrorMessage"] = "Select Vlaue in Dropdown";
			}
			var designations = _designationService.GetAllDesignation().Result;
			ViewBag.Designations = designations;
			var modules = _moduleMasterService.GetAllModuleMaster().Result;
			ViewBag.Modules = modules;
			return View(permissionDto);
		}
		public IActionResult ViewPermission()
		{
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> GetAllPermissions()
		{
			try
			{
				if (_globalClass.Token != null)
				{
					var permissionDto = new PermissionDto();
					var permission = await _permissionService.GetAllPermissions();
					if (permission != null)
					{
						return Json(new { result = "Success", data = permission });
					}
				}
				return Json(new { data = (EmployeeDto)null });
			}
			catch (Exception ex)
			{
				return Json(new { data = (EmployeeDto)null, message = ex.Message });
			}
		}
		public async Task<IActionResult> UpdatePermission(int Id)
		{
			if (_globalClass.Token != null)
			{
				var permission = await _permissionService.GetPermissionById(Id);
				if (permission != null)
				{
					ViewBag.appUrl = applicationURL.url;
					var designation = await _designationService.GetAllDesignation();
					ViewBag.Designation = designation;
					var module = await _moduleMasterService.GetAllModuleMaster();
					ViewBag.Module = module;
					return View(permission);
				}
				else
				{
					var designation = await _designationService.GetAllDesignation();
					ViewBag.Designation = designation;
					var module = await _moduleMasterService.GetAllModuleMaster();
					ViewBag.Module = module;
				}
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> UpdatePermission(PermissionDto permissionDto)
		{
			if (ModelState.IsValid)
			{
				var permission = new PermissionDto
				{
					DesignationId = permissionDto.DesignationId,
					ModuleMasterId = permissionDto.ModuleMasterId,
					CanAccess = permissionDto.CanAccess
				};
			}
			var result = await _permissionService.UpdatePermission(permissionDto, permissionDto.PermissionId);
			if (result != null)
			{
				TempData["Message"] = "Permission updated successfully.";
			}
			else
			{
				ViewData["errormsg"] = "Permission Not Updated";
			}

			return RedirectToAction("ViewPermission");
		}
		public async Task<IActionResult> DeletePermission(int Id)
		{
			if (_globalClass.Token != null)
			{
				var result = await _permissionService.DeletePermission(Id);
				if (result != 0)
				{

					TempData["Message"] = "Permission Deleted successfully.";
				}
				else
				{
					TempData["errormsg"] = "Permission not found.";
				}
			}
			return RedirectToAction("ViewPermission");
		}
	}
}

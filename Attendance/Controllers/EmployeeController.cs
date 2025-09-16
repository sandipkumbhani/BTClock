using Attendance.Application.Interface;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Attendance.Domain.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;

namespace Attendance.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
	{
		private readonly IConfiguration _configuration;
		private ApplicationURL applicationURL;
		private readonly GlobalClass _globalClass;
		private readonly IEmployeeService _employeeService;
		public EmployeeController(IConfiguration configuration, GlobalClass globalClass, IEmployeeService employeeService)
		{
			_configuration = configuration;
			applicationURL = new ApplicationURL(configuration);
			_globalClass = globalClass;
			_employeeService = employeeService;
		}
		public IActionResult Employee()
		{
			if (_globalClass.Token != null)
			{
				var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_globalClass.Token);
				var claims = UserUtility.addClaimstoUser(HttpContext, jwt.Claims);
				string currentPage = "Employee";
				ViewBag.appUrl = applicationURL.url;
				return View();
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Employee(EmployeeDto employeedto)
		{
			ViewBag.appUrl = applicationURL.url;
			if (ModelState.IsValid)
			{
				var employee =new EmployeeDto
				{
					Name=employeedto.Name,
					Email = employeedto.Email,
					Password = employeedto.Password,
					MobileNo = employeedto.MobileNo,
					//Designation = employeedto.Designation,
					DesignationId = employeedto.DesignationId,
					DepartmentId = employeedto.DepartmentId,
					ManagerId = employeedto.ManagerId,
					DateOfJoining = employeedto.DateOfJoining,
					DateofLeaving = employeedto.DateofLeaving,
					CreatedAt = DateTime.Now,
					CreatedBy = employeedto.EmployeeId,
					IsActive = true
				};
				var result = await _employeeService.AddEmployee(employee);
				if (result != null)
				{
					if (result != null)
					{
						ViewData["Message"] = "Employee added successfully.";
						ModelState.Clear();
						return View();
					}
					else
					{
						ViewData["ErrorMessage"] = "Employee Not Added";
						return View();
					}
				}
				else
				{
					ViewData["ErrorMessage"] = "An error occurred while processing your request.";
					return View();
				} 
			}
			ViewBag.appUrl = applicationURL.url;
			return View(employeedto);
		}
		
		public IActionResult ViewEmployee()
		{
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> GetAllEmployee()
		{
			try
			{
				if (_globalClass.Token != null)
				{
					var employeedto = new EmployeeDto();
					var employee = await _employeeService.GetAllEmployee();
					if (employee != null)
					{
						return Json(new { result = "Success",data = employee });
					}
				}
				return Json(new { data = (EmployeeDto)null });
			}
			catch (Exception ex)
			{
				return Json(new { data = (EmployeeDto)null, message = ex.Message });
			}
		}
		public async Task<IActionResult> UpdateEmployee(int employeeId)
		{
			if (_globalClass.Token != null)
			{
				var employee = await _employeeService.GetEmployeeById(employeeId);
				 if (employee != null)
				{
					ViewBag.appUrl = applicationURL.url;
					return View(employee);
				}
				else
				{
					ViewData["ErrorMessage"] = "Employee Not Found";
					return View();
				}
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> UpdateEmployee(EmployeeDto employeedto)
		{
			if(ModelState.IsValid) 
			{	
			   var employeelist = await _employeeService.GetEmployeeById(employeedto.EmployeeId);
				var employeemodel = new EmployeeDto
				{
					Name = employeedto.Name,
					Email = employeedto.Email,
					MobileNo = employeedto.MobileNo,
					DesignationId = employeedto.DesignationId,
					DepartmentId = employeedto.DepartmentId,
					ManagerId = employeedto.ManagerId,
					DateOfJoining = employeedto.DateOfJoining,
					DateofLeaving = employeedto.DateofLeaving,
					UpdatedAt = DateTime.Now,
					UpdatedBy = employeedto.EmployeeId,
					CreatedAt = employeelist.CreatedAt,
					CreatedBy = employeelist.CreatedBy,
					Password = employeelist.Password
				};
				var result = await _employeeService.UpdateEmployee(employeemodel, employeedto.EmployeeId);


				if (result != null)
				{
					if (result != null)
					{
						TempData["Message"] = "Employee updated successfully.";
						return RedirectToAction("ViewEmployee");
					}
					else
					{
						ViewBag.ErrorMessage = "Employee Not Updated";
						return View(employeedto);
					}
				}
				else
				{
					ViewBag.ErrorMessage = "An error occurred while processing your request.";
					return View();
				}
			}
			ViewBag.appUrl = applicationURL.url;
			return View(employeedto);
		}
		public async Task<IActionResult> DeleteEmployee(int id)
		{
			try
			{
				if (_globalClass.Token != null)
				{
					var result = await _employeeService.DeleteEmployee(id);
					if (result > 0)
					{
						TempData["Message"] = "Employee Deleted successfully.";
						return RedirectToAction("ViewEmployee");
					}
					else
					{
						TempData["errormsg"] = "Employee not found.";
						return RedirectToAction("ViewEmployee");
					}
				}
				return Json(new { success = false, message = "Unauthorized access." });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "An error occurred: " + ex.Message });
			}
		}
	}
}

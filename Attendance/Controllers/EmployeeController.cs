using Attendance.Application.Interface;
using Attendance.Application.service;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;

namespace Attendance.Controllers
{
	[Authorize]
	public class EmployeeController : BaseAdminController
    {
		private readonly IConfiguration _configuration;
		private ApplicationURL applicationURL;
		private readonly GlobalClass _globalClass;
		private readonly IEmployeeService _employeeService;
		private readonly IDesignationService _designationService;
		private readonly IDepartmentService _departmentService;
    private readonly IMenuMasterService _menuService;
    private readonly IUserMenuMappingService _userMenuMappingService;
    public EmployeeController(IConfiguration configuration, GlobalClass globalClass, IEmployeeService employeeService, IDesignationService designationService, IDepartmentService departmentService, IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService) : base(menuService, userMenuMappingService)
    {
        _configuration = configuration;
        applicationURL = new ApplicationURL(configuration);
        _globalClass = globalClass;
        _employeeService = employeeService;
        _designationService = designationService;
        _departmentService = departmentService;
        _menuService = menuService;
        _userMenuMappingService = userMenuMappingService;
    }
    public IActionResult Employee()
		{
			if (_globalClass.Token != null)
			{
				var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_globalClass.Token);
				var claims = UserUtility.addClaimstoUser(HttpContext, jwt.Claims);
				string currentPage = "Add Employee";
                var canAccess = UserUtility.CanAccessMenu(HttpContext, currentPage);

                if (canAccess)
                {
                    var Employee = claims.Claims.FirstOrDefault(x => x.Type == "EmployeeId");
                    int EmployeeId = Employee != null ? (!string.IsNullOrEmpty(Employee.Value) ? Convert.ToInt32(Employee.Value) : 0) : 0;
                    ViewBag.EmployeeId = EmployeeId;
                    ViewBag.appUrl = applicationURL.url;
                    var designation = _designationService.GetAllDesignation().Result;
                    ViewBag.Designation = designation;
                    var department = _departmentService.GetAllDepartments().Result;
                    ViewBag.Department = department;
                    var employee = _employeeService.GetAllEmployee().Result;
                    ViewBag.Employee = employee;
                    ViewBag.appUrl = applicationURL.url;
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
        public async Task<IActionResult> Employee(EmployeeDto employeedto)
        {
            ViewBag.appUrl = applicationURL.url;
            var employeeid = UserUtility.GetUserId(HttpContext);

            if (ModelState.IsValid)
            {
                var existingEmployee = await _employeeService.GetAllEmployee();
                var isEmailExists = existingEmployee.Any(e => e.Email == employeedto.Email && e.IsActive == true);

                if (isEmailExists)
                {
                    ViewData["ErrorMessage"] = "Email already exists. Please use a different email.";
                }
                else
                {
                    var employee = new EmployeeDto
                    {
                        Name = employeedto.Name,
                        Email = employeedto.Email,
                        Password = employeedto.Password,
                        MobileNo = employeedto.MobileNo,
                        DesignationId = employeedto.DesignationId,
                        DepartmentId = employeedto.DepartmentId,
                        ManagerId = employeedto.ManagerId,
                        DateOfJoining = employeedto.DateOfJoining,
                        DateofLeaving = employeedto.DateofLeaving,
                        CreatedAt = DateTime.Now,
                        CreatedBy = Convert.ToInt16(employeeid),
                        IsActive = true
                    };
                    var result = await _employeeService.AddEmployee(employee);

                    if (result != null)
                    {
                        var employeeMenus = await _userMenuMappingService.GetUserMenuById(employee.EmployeeId);

                        if (employeeMenus.Any())
                        {
                            foreach (var menu in employeeMenus)
                            {
                                var mappingDto = new UserMenuMappingDto
                                {
                                    EmployeeId = employee.EmployeeId,
                                    MenuMasterMenuid = menu.MenuMasterMenuid
                                };
                                await _userMenuMappingService.AddUserMenuMapping(mappingDto);
                            }
                        }
                        else
                        {
                            var menus = await _menuService.GetAllMenuMasters();
                            var defaultMenus = menus.Where(m => m.isDefault).ToList();

                            foreach (var menu in defaultMenus)
                            {
                                var mappingDto = new UserMenuMappingDto
                                {
                                    EmployeeId = employee.EmployeeId,
                                    MenuMasterMenuid = menu.Menuid
                                };
                                await _userMenuMappingService.AddUserMenuMapping(mappingDto);
                            }
                        }
                        ViewData["Message"] = "Employee added successfully.";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Employee Not Added";
                    }
                }
            }

            var designation = await _designationService.GetAllDesignation();
            ViewBag.Designation = designation;
            var department = await _departmentService.GetAllDepartments();
            ViewBag.Department = department;
            var employeeList = await _employeeService.GetAllEmployee();
            ViewBag.Employee = employeeList;

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
						return Json(new { result = "Success", data = employee });
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
					var designation = await _designationService.GetAllDesignation();
					ViewBag.Designation = designation;
					var department = await _departmentService.GetAllDepartments();
					ViewBag.Department = department;
					var manager = await _employeeService.GetAllEmployee();
					ViewBag.Manager = manager;
					return View(employee);
				}
				else
				{
					ViewData["ErrorMessage"] = "Employee Not Found";
					var designation = await _designationService.GetAllDesignation();
					ViewBag.Designation = designation;
					var department = await _departmentService.GetAllDepartments();
					ViewBag.Department = department;
					var manager = await _employeeService.GetAllEmployee();
					ViewBag.Manager = manager;
				}
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> UpdateEmployee(EmployeeDto employeedto)
		{
			if (ModelState.IsValid)
			{
				var existingEmployee = await _employeeService.GetAllEmployee();
				var isEmailExists = existingEmployee.Any(e => e.Email == employeedto.Email && e.EmployeeId != employeedto.EmployeeId && e.IsActive == true);
				if (isEmailExists) {
					ViewBag.ErrorMessage = "Email already exists. Please use a different email.";
				}
				else
				{

				var employeeid = UserUtility.GetUserId(HttpContext);
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
					UpdatedBy = Convert.ToInt16(employeeid),
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
			}
			ViewBag.appUrl = applicationURL.url;
			var designation = await _designationService.GetAllDesignation();
			ViewBag.Designation = designation;
			var department = await _departmentService.GetAllDepartments();
			ViewBag.Department = department;
			var manager = await _employeeService.GetAllEmployee();
			ViewBag.Manager = manager;

			var employee = await _employeeService.GetEmployeeById(employeedto.EmployeeId);
			return View(employee);
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

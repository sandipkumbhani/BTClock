using Attendance.Api.Contracts.Request;
using Attendance.Application.Interface;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
	public class RegisterController : Controller
	{
		private readonly IUserRegisterService _userRegisterService;
		public RegisterController(IUserRegisterService userRegisterService)
		{
			_userRegisterService = userRegisterService;
		}
		[HttpGet]
		public IActionResult URegister()
		{
			return View();
		}

	   [HttpPost]
	   public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
	   {
			   if (!ModelState.IsValid)
			   {
					   ViewData["ErrorMessage"] = "Invalid data.";
					   return View("URegister");
			   }

			   if (userRegisterDto != null)
			   {
					   
					   if (userRegisterDto.Password != userRegisterDto.ConfirmPassword)
					   {
							   ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
							   return View("URegister");
					   }

					   var userRegister = new Users
					   {
						   Email = userRegisterDto.Email ?? string.Empty,
						   Password = userRegisterDto.Password ?? string.Empty
					   };

					   try
					   {
							   var result = await _userRegisterService.RegisterUserAsync(userRegister);
							   TempData["SuccessMessage"] = "Registration successful!";
							   ModelState.Clear();
							   return View("URegister");
					   }
					   catch (InvalidOperationException ex)
					   {
							   ModelState.AddModelError("Email", ex.Message);
							   return View("URegister");
					   }
			   }
			   return View("URegister");
	   }

	}
}

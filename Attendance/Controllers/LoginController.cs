using Attendance.Application.Interface;
using Attendance.Domain.Models;
using Attendance.Domain.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Attendance.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginServices _loginServices;
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;

        public LoginController(ILogger<LoginController> logger, ILoginServices loginServices, IConfiguration configuration)
        {
            _logger = logger;
            _loginServices = loginServices;
            _configuration = configuration;
            applicationURL = new ApplicationURL(configuration);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Employee model, string? returnUrl)
        {   
            if (ModelState.IsValid)
            {
                var token = await _loginServices.Login(model);
                if (!string.IsNullOrEmpty(token))
                {
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTimeOffset.UtcNow.AddHours(12),
                        SameSite = SameSiteMode.Strict,
                        Secure = true
                    });
                    var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    var email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "unique_name")?.Value;
                    var employeeId = jwt.Claims.FirstOrDefault(c => c.Type == "EmployeeId" || c.Type == "unique_name")?.Value;
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, email ?? model.Email),
                        new Claim("EmployeeId", employeeId ?? ""),
                        new Claim(ClaimTypes.UserData, employeeId)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }
            }
            ViewData["LoginMessage"] = "Invalid username or password..!";
            ViewBag.appUrl = applicationURL.url;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }
    }
}

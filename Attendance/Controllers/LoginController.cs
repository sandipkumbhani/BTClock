using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;


namespace Attendance.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginServices _loginServices;
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;

        public LoginController(ILogger<LoginController> logger, ILoginServices loginServices, IConfiguration configuration, IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService)
        {
            _logger = logger;
            _loginServices = loginServices;
            _configuration = configuration;
            applicationURL = new ApplicationURL(configuration);
            _menuService = menuService;
            _userMenuMappingService = userMenuMappingService;
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
                        //Expires = DateTimeOffset.UtcNow.AddHours(12),
                        SameSite = SameSiteMode.Strict,
                        Secure = true
                    });
                    var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    var email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "unique_name")?.Value;
                    var employeeId = jwt.Claims.FirstOrDefault(c => c.Type == "EmployeeId" || c.Type == "unique_name")?.Value;
                    var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, email ?? model.Email),
                        new Claim("EmployeeId", employeeId ?? ""),
                        new Claim(ClaimTypes.UserData, employeeId),
                        new Claim(ClaimTypes.Role,role!.ToString())
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    var existingMenus = await _userMenuMappingService.GetUserMenuById(Convert.ToInt32(employeeId));
                    if (!existingMenus.Any())
                    {
                        var allMenus = await _menuService.GetAllMenuMasters();
                        var defaultMenus = allMenus.Where(m => m.isDefault).ToList();

                        foreach (var menu in defaultMenus)
                        {
                            var mappingDto = new UserMenuMappingDto
                            {
                                EmployeeId = Convert.ToInt32(employeeId),
                                MenuMasterMenuid = menu.Menuid
                            };

                            await _userMenuMappingService.AddUserMenuMapping(mappingDto);
                        }
                    }
                    var _menus = new List<UserMenuMappingDto>();
                    var menulist = await _userMenuMappingService.GetAll();
                    if (role == "Admin")
                    {
                        _menus = menulist.ToList();
                    }
                    else
                    {
                        _menus = menulist.Where(x => x.EmployeeId == Convert.ToUInt32(employeeId)).ToList();
                    }
                    var menus = _menus.Select(x => x.MenuMaster.Menuname).ToList();
                    string menuJson = JsonSerializer.Serialize(menus);
                    Response.Cookies.Append("MenuAccess", menuJson, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        //Expires = DateTime.UtcNow.AddHours(24)
                    });
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

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}

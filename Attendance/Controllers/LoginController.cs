using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

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
                    SameSite = SameSiteMode.Strict,
                    Secure = true
                });

                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "unique_name")?.Value;
                var userId = jwt.Claims.FirstOrDefault(c => c.Type == "UserId" || c.Type == "unique_name")?.Value;
                var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email ?? model.Email),
                    new Claim("UserId", userId ?? ""),
                    new Claim(ClaimTypes.UserData, userId),
                    new Claim(ClaimTypes.Role, role!)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                var existingMenus = await _userMenuMappingService.GetUserMenuById(Convert.ToInt32(userId));
                if (!existingMenus.Any())
                {
                    var allMenus = await _menuService.GetAllMenuMasters();

                    if (allMenus == null)
                    {
                        _logger.LogWarning("GetAllMenuMasters returned null for userId: {UserId}", userId);
                        allMenus = new List<menuMasterDto>();
                    }

                    if (role == "Admin" || role == "SuperAdmin")
                    {
                        foreach (var menu in allMenus)
                        {
                            var mappingDto = new UserMenuMappingDto
                            {
                                UserId = Convert.ToInt32(userId),
                                MenuItemId = menu.Menuid
                            };
                            await _userMenuMappingService.AddUserMenuMapping(mappingDto);
                        }
                    }
                    else
                    {
                        var defaultMenus = allMenus.Where(m => m.isDefault).ToList();
                        foreach (var menu in defaultMenus)
                        {
                            var mappingDto = new UserMenuMappingDto
                            {
                                UserId = Convert.ToInt32(userId),
                                MenuItemId = menu.Menuid
                            };
                            await _userMenuMappingService.AddUserMenuMapping(mappingDto);
                        }
                    }
                }

                var allUserMenuMappings = await _userMenuMappingService.GetAll();
                var userMenus = new List<UserMenuMappingDto>();

                if (role == "Admin" || role == "SuperAdmin")
                {
                    userMenus = allUserMenuMappings.ToList();  
                }
                else
                {
                    userMenus = allUserMenuMappings
                        .Where(x => x.UserId == Convert.ToInt32(userId))
                        .ToList();  // Other users get only their specific mappings
                }

                var menuNames = userMenus
                    .Where(m => m.MenuItem != null)
                    .Select(x => x.MenuItem.MenuName)
                    .ToList();
                string menuJson = JsonSerializer.Serialize(menuNames);
                Response.Cookies.Append("MenuAccess", menuJson, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
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

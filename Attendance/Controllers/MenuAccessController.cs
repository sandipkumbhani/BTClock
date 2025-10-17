using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Attendance.Controllers
{
    [Authorize]
    public class MenuAccessController : BaseAdminController
    {
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuItemService _menuItemService;

        public MenuAccessController(IConfiguration configuration, GlobalClass globalClass, IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService, IMenuItemService menuItemService) : base(menuService, userMenuMappingService, menuItemService)
        {
            _configuration = configuration;
            applicationURL = new ApplicationURL(configuration);
            _globalClass = globalClass;
            _menuService = menuService;
            _userMenuMappingService = userMenuMappingService;
            _menuItemService = menuItemService;
        }
        public IActionResult Redirect()
        {
            if (_globalClass.Token != null)
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_globalClass.Token);
                var menus = ((List<MenuItemDto>)ViewBag.MainMenu);

                if (menus.Count() > 0)
                {
                    var redirectTo = menus
                        .Where(menu => menu.MenuMaster != null)
                        .FirstOrDefault()?.MenuMaster?.Path;

                    if (!string.IsNullOrEmpty(redirectTo))
                    {
                        ViewBag.appUrl = applicationURL.url;
                        return LocalRedirect(redirectTo);
                    }
                    else
                    {
                        return RedirectToAction("AccessDenied", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

    }
}

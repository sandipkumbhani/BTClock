using Attendance.Application.Interface;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Attendance.Controllers
{
    public class BaseAdminController : Controller
    {
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;

        public BaseAdminController(IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService)
        {
            _menuService = menuService;
            _userMenuMappingService = userMenuMappingService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            //var role = UserUtility.GetDesignation(HttpContext);
            //if (role == "Admin")
            //{
            //    var menulist = await _menuService.GetAllMenuMasters();
            //    var homecareMenuList = menulist.Where(x => x.ModuleMasterId == 5).ToList();
            //    var topLevelItems = homecareMenuList.Where(m => m.ParentId == 0 || m.ParentId == null).ToList();
            //    foreach (var item in topLevelItems)
            //    {
            //        item.Children = homecareMenuList.Where(m => m.ParentId == item.Menuid).ToList();
            //    }
            //    ViewBag.MainMenu = topLevelItems;
            //    await next();
            //}
            //else
            //{

                var employeeID = UserUtility.GetUserId(HttpContext);
                var userMenus = await _userMenuMappingService.GetUserMenuById(Convert.ToInt32(employeeID));
                var userMenuList = userMenus.Where(x => x.MenuMaster?.ModuleMasterId == 3).Select(x => x.MenuMasterMenuid);
                var menulist = await _menuService.GetAllMenuMasters();
                var UserMenuAccesslist = menulist.Where(x => userMenuList.Contains(x.Menuid)).ToList();

                var topLevelItems = UserMenuAccesslist.Where(m => m.ParentId == 0 || m.ParentId == null).ToList();
                foreach (var item in topLevelItems)
                {
                    item.Children = UserMenuAccesslist.Where(m => m.ParentId == item.Menuid).ToList();
                }

                ViewBag.MainMenu = topLevelItems;
                await next();
            //}
        }
    }
}

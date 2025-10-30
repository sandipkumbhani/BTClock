using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;

namespace Attendance.Domain.Utility
{
    public static class UserUtility
    {
        public static ClaimsPrincipal AddClaimsToUser(HttpContext context, IEnumerable<Claim> claims)
        {
            var claimsIdentity = new ClaimsIdentity(claims, "UserAuthentication");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            context.User = claimsPrincipal;
            return claimsPrincipal;
        }

        //public static bool CanAccessMenu(HttpContext context, string actionName)
        //{
        //    var menulist = context.Request.Cookies["MenuAccess"];
        //    if (menulist != null)
        //    {
        //        var menu = JsonSerializer.Deserialize<List<string>>(menulist);
        //        if (menu != null && menu.Contains(actionName))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public static bool CanAccessMenu(HttpContext context, string actionName)
        {
            if (context == null || string.IsNullOrWhiteSpace(actionName))
                return false;

            int moduleId = context.Items["CurrentModuleId"] is int id ? id :
                           int.TryParse(context.Request.Cookies["CurrentModuleId"] ?? context.Request.Query["moduleMasterId"], out var mid) ? mid : 0;

            string cookieKey = moduleId > 0 ? $"MenuAccess_Module{moduleId}" : "MenuAccess";

            //var menuJson = context.Request.Cookies[cookieKey];
            //    if (string.IsNullOrEmpty(menuJson)) return false;

            //    var menuList = JsonSerializer.Deserialize<List<string>>(menuJson);
            //    if (menuList == null || !menuList.Any()) return false;
            var menuList = context.Items[cookieKey] as List<string>
                ?? JsonSerializer.Deserialize<List<string>>(context.Request.Cookies[cookieKey] ?? "[]");

            if (menuList == null || menuList.Count == 0)
                return false;

            return true;
        }

        public static string GetUserId(HttpContext context)
        {
            ClaimsPrincipal currentUser = context.User;
            Claim userIdClaim = currentUser?.FindFirst(ClaimTypes.UserData);
            return userIdClaim?.Value;
        }

        public static string GetRole(HttpContext context)
        {
            ClaimsPrincipal currentUser = context.User;
            Claim userIdClaim = currentUser?.FindFirst(ClaimTypes.Role);
            return userIdClaim?.Value;
        }
        public static string GetCompanyId(HttpContext context)
        {
            ClaimsPrincipal currentUser = context.User;
            Claim userIdClaim = currentUser?.FindFirst(ClaimTypes.UserData);
            return userIdClaim?.Value;
        }

    }
}

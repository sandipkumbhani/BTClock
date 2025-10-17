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

        public static bool CanAccessMenu(HttpContext context, string actionName)
        {
            var menulist = context.Request.Cookies["MenuAccess"];
            if (!string.IsNullOrEmpty(menulist))
            {
                var menu = JsonSerializer.Deserialize<List<string>>(menulist);
                if (menu != null && menu.Contains(actionName))
                    return true;
            }
            return false;
        }

        public static int GetUserId(HttpContext context)
        {
            ClaimsPrincipal currentUser = context.User;
            Claim userIdClaim = currentUser?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        public static int GetRoleId(HttpContext context)
        {
            ClaimsPrincipal currentUser = context.User;
            Claim roleClaim = currentUser?.FindFirst(ClaimTypes.Role);
            return roleClaim != null ? int.Parse(roleClaim.Value) : 0;
        }
    }
}

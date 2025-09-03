using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;

namespace Attendance.Domain.Utility
{
    public static class UserUtility
    {
        public static ClaimsPrincipal addClaimstoUser(HttpContext context, IEnumerable<Claim> claims)
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
        //        if (menu != null)
        //        {
        //            if (menu.Contains(actionName))
        //            {
        //                return true;
        //            }
        //        }

        //    }
        //    return false;
        //}
        public static int GetUserId(HttpContext context)
        {

            ClaimsPrincipal currentUser = context.User;
            Claim userIdClaim = currentUser?.FindFirst(ClaimTypes.UserData);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}


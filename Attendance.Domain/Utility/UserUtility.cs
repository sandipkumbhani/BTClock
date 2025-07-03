using System.Security.Claims;

namespace Attendance.Domain.Utility
{
	public static class UserUtility
	{
		public static int GetUserId(ClaimsPrincipal user)
		{
			if (user == null || !user.Identity.IsAuthenticated)
			{
				return 0;
			}

			var userId = user.Claims
				.Where(c => c.Type == ClaimTypes.NameIdentifier)
				.Select(c => c.Value)
				.SingleOrDefault();

			return string.IsNullOrEmpty(userId) ? 0 : Convert.ToInt32(userId);
		}
	}
}


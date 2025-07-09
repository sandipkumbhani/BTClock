using Attendance.Application.Interface;
using Attendance.Application.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace Attendance.Application.Extension
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			
			services.AddScoped<IUserRegisterService, UserRegisterService>();
			services.AddScoped<IAttendanceService, AttendanceService>();

			return services;
		}
	}
}

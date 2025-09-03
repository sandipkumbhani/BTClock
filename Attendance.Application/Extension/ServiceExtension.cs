using Attendance.Application.Interface;
using Attendance.Application.Provider;
using Attendance.Application.service;
using Microsoft.Extensions.DependencyInjection;

namespace Attendance.Application.Extension
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			
			services.AddScoped<IAttendanceService, AttendanceService>();
			services.AddScoped<ILoginServices, LoginServices>();

            return services;
		}
	}
}

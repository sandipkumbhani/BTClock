using Attendance.Domain.Interfaces;
using Attendance.Infrastructure.Efcore.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace Attendance.Infrastructure.Efcore.Extensions
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddAttendanceServices(this IServiceCollection services)
		{
			services.AddScoped<IUserRegisterRepository, UserRegisterRepository>();
			services.AddScoped<IAttendanceRepository, Attendance.Infrastructure.Efcore.Providers.AttendanceRepository>();

			return services;
		}
	}
}

using Attendance.Domain.Interfaces;
using Attendance.Infrastructure.Efcore.Providers;
using Microsoft.Extensions.DependencyInjection;


namespace Attendance.Infrastructure.Efcore.Extensions
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddEfcoreInfrastructureService(this IServiceCollection services)
		{
			services.AddScoped<IAttendanceAdaptor,AttendanceAdaptor>();
			services.AddScoped<ILoginAdaptor, LoginAdaptor>();

            return services;
		}
	}
}

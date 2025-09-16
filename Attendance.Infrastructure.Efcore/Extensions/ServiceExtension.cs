using Attendance.Domain.Interfaces;
using Attendance.Infrastructure.Efcore.Providers;
using Attendance.Infrastructure.Provider;
using Microsoft.Extensions.DependencyInjection;


namespace Attendance.Infrastructure.Efcore.Extensions
{
	public static class ServiceExtension
	{
		public static IServiceCollection AddEfcoreInfrastructureService(this IServiceCollection services)
		{
			services.AddScoped<IAttendanceAdaptor,AttendanceAdaptor>();
			services.AddScoped<ILoginAdaptor, LoginAdaptor>();
			services.AddScoped<IEmployeeAdaptor, EmployeeAdaptor>();
			services.AddScoped<IDepartmentAdaptor, DepartmentAdaptor>();
			services.AddScoped<IDesignationAdaptor, DesignationAdaptor>();
			services.AddScoped<ILeaveMasterAdaptor, LeaveMasterAdaptor>();
			services.AddScoped<IModuleMasterAdaptor, ModuleMasterAdaptor>();
			services.AddScoped<IMenuMasterAdaptor, MenuMasterAdaptor>();
			services.AddScoped<IUserMenuMappingAdaptor, UserMenuMappingAdaptor>();
			services.AddScoped<ILeaveTransactionAdaptor, LeaveTransactionAdaptor>();
			services.AddScoped<ILeaveBalanceAdaptor, LeaveBalanceAdaptor>();
			services.AddScoped<ILeaveAssignmentAdaptor, LeaveAssignmentAdaptor>();
			services.AddScoped<IHolidayMasterAdaptor, HolidayMasterAdaptor>();
			return services;
		}
	}
}

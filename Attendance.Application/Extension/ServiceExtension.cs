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
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDesignationService, DesignationService>();
            services.AddScoped<ILeaveMasterService, LeaveMasterService>();
            services.AddScoped<IModuleMasterService, ModuleMasterService>();
            services.AddScoped<IMenuMasterService, MenuMasterService>();
            services.AddScoped<IUserMenuMappingService, UserMenuMappingService>();
            services.AddScoped<ILeaveTransactionService, LeaveTransactionService>();
            services.AddScoped<ILeaveBalanceService, LeaveBalanceService>();
            services.AddScoped<ILeaveAssignmentService, LeaveAssignmentService>();
            return services;
        }
    }
}

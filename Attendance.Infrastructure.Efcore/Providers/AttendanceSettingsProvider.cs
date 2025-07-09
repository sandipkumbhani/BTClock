using Attendance.Application.Interface;
using Microsoft.Extensions.Configuration;

namespace Attendance.Infrastructure.Efcore.Providers
{
    public class AttendanceSettingsProvider : IAttendanceSettingsProvider
    {
        public bool AllowMultipleClockInOutPerDay { get; }

        public AttendanceSettingsProvider(IConfiguration configuration)
        {
            AllowMultipleClockInOutPerDay = configuration.GetSection("AttendanceSettings").GetValue<bool>("AllowMultipleClockInOutPerDay");
        }
    }
}

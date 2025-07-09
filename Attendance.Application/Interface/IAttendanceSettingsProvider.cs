namespace Attendance.Application.Interface
{
    public interface IAttendanceSettingsProvider
    {
        bool AllowMultipleClockInOutPerDay { get; }
    }
}

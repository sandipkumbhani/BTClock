using Attendance.Application.Interface;
using Quartz;

public class AttendanceQuartzJob : IJob
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<AttendanceQuartzJob> _logger;

	public AttendanceQuartzJob(IServiceProvider serviceProvider, ILogger<AttendanceQuartzJob> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		_logger.LogInformation("Running Attendance Quartz Job...");

		using (var scope = _serviceProvider.CreateScope())
		{
			var attendanceService = scope.ServiceProvider.GetRequiredService<IAttendanceService>();
			var now = DateTime.Now;
			// Get all attendance records where ClockOut is null (still clocked in)
			var recordsToUpdate = await attendanceService.GetAllOpenAttendancesAsync(); // You need to implement this method to get all records with ClockOut == null

			foreach (var record in recordsToUpdate)
			{
				
				record.ClockOut = now;
				await attendanceService.ClockOutJobAsync(record.EmployeeId, record.ClockOut.Value);
				_logger.LogInformation($"User {record.EmployeeId} automatically clocked out at {record.ClockOut.Value}");
				
			}
		}

		_logger.LogInformation("Attendance Quartz Job completed.");
	}
}

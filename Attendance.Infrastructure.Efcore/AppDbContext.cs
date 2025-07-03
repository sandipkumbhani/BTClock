using Microsoft.EntityFrameworkCore;
using Attendance.Domain.Models;

namespace Attendance.Infrastructure.Efcore
{
   public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Users> Users { get; set; }
		public DbSet<AttendanceRecord> AttendanceRecords { get; set; }

	}
}

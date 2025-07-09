using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Efcore.Repositories
{
	public class UserRegisterRepository : IUserRegisterRepository
	{
		private readonly AppDbContext _context;

		public UserRegisterRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Users> AddUserRegisterAsync(Users userRegister)
		{
			_context.Users.Add(userRegister);
			await _context.SaveChangesAsync();
			return userRegister;
		}

		public async Task<bool> EmailExitsAsync(string email)
		{
			return await _context.Users.AnyAsync(u => u.Email == email);
		}

		public async Task<IEnumerable<Users>> GetAllUsersAsync()
		{
			return await _context.Users.ToListAsync();
		}
	}
}

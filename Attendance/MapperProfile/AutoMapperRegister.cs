using Attendance.Api.Contracts.Request;
using Attendance.Domain.Models;
using AutoMapper;

namespace Attendance.MapperProfile
{
	public class AutoMapperRegister : Profile
	{
		public AutoMapperRegister()
		{
			
			CreateMap<UserRegisterDto, Users>();
			CreateMap<Users, UserRegisterDto>();
				
		}
	}
}

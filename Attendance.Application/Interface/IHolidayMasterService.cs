using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface IHolidayMasterService
    {
		Task<HolidayMasterDto> GetHolidayMasterById(int Id);
		Task<List<HolidayMasterDto>> GetAllHolidayMaster();
		Task<string> AddHolidayMaster(HolidayMasterDto holidayMasterDto);
		Task<string> UpdateHolidayMaster(HolidayMasterDto holidayMasterDto, int Id);
		Task<int> DeleteHolidayMaster(int Id);
	}
}

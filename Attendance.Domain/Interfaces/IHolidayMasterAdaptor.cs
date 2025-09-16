using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface IHolidayMasterAdaptor
    {
		Task<HolidayMasterDto> GetHolidayMasterByIdAsync(int Id);
        Task<List<HolidayMasterDto>> GetAllHolidayMasterAsync();
		Task<string> AddHolidayMasterAsync(HolidayMasterDto holidayMasterDto);
		Task<string> UpdateHolidayMasterAsync(HolidayMasterDto holidayMasterDto, int Id);
		Task<int> DeleteHolidayMasterAsync(int Id);
	}
}

using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;

namespace Attendance.Application.service
{
   public  class HolidayMasterService : IHolidayMasterService
	{
		private readonly IHolidayMasterAdaptor _holidayMasterAdaptor;
		public HolidayMasterService(IHolidayMasterAdaptor holidayMasterAdaptor)
		{
			_holidayMasterAdaptor = holidayMasterAdaptor;
		}
		public async Task<List<HolidayMasterDto>> GetAllHolidayMaster()
		{
			return await _holidayMasterAdaptor.GetAllHolidayMasterAsync();
		}
		public async Task<HolidayMasterDto> GetHolidayMasterById(int holidayMasterId)
		{
			return await _holidayMasterAdaptor.GetHolidayMasterByIdAsync(holidayMasterId);
		}
		public async Task<string> AddHolidayMaster(HolidayMasterDto holidayMasterDto)
		{
			return await _holidayMasterAdaptor.AddHolidayMasterAsync(holidayMasterDto);
		}
		public async Task<string> UpdateHolidayMaster(HolidayMasterDto holidayMasterDto, int Id)
		{
			return await _holidayMasterAdaptor.UpdateHolidayMasterAsync(holidayMasterDto, Id);
		}
		public async Task<int> DeleteHolidayMaster(int Id)
		{
			return await _holidayMasterAdaptor.DeleteHolidayMasterAsync(Id);
		}
	}
}

using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;

namespace Attendance.Application.service
{
   public class LeaveMasterService:ILeaveMasterService
    {
        private readonly ILeaveMasterAdaptor _leaveMasterAdaptor;
		public LeaveMasterService(ILeaveMasterAdaptor leaveMasterAdaptor)
		{
			_leaveMasterAdaptor = leaveMasterAdaptor;
		}
		public async Task<List<LeaveMasterDto>> GetAllLeaveMasters(LeaveMasterDto leaveMasterDto)
		{
			return await _leaveMasterAdaptor.GetAllLeaveMastersAsync(leaveMasterDto);
		}
		public async Task<LeaveMasterDto> GetLeaveMasterById(int Id)
		{
			return await _leaveMasterAdaptor.GetLeaveMasterByIdAsync(Id);
		}
		public async Task<string> AddLeaveMaster(LeaveMasterDto leaveMasterDto)
		{
			return await _leaveMasterAdaptor.AddLeaveMasterAsync(leaveMasterDto);
		}
		public async Task<string> UpdateLeaveMaster(LeaveMasterDto leaveMasterDto, int Id)
		{
			return await _leaveMasterAdaptor.UpdateLeaveMasterAsync(leaveMasterDto, Id);
		}
		public async Task<int> DeleteLeaveMaster(int Id)
		{
			return await _leaveMasterAdaptor.DeleteLeaveMasterAsync(Id);
		}
	}
}

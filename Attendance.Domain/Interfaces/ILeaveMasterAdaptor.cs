using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface ILeaveMasterAdaptor
    {
		Task<LeaveMasterDto> GetLeaveMasterByIdAsync(int leaveMasterId);
        Task<List<LeaveMasterDto>> GetAllLeaveMastersAsync();
		Task<string> AddLeaveMasterAsync(LeaveMasterDto leaveMasterDto);
		Task<string> UpdateLeaveMasterAsync(LeaveMasterDto leaveMasterDto, int leaveMasterId);
		Task<int> DeleteLeaveMasterAsync(int leaveMasterId);
	}
}

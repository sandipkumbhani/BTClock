using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface ILeaveMasterService
    {
        Task<LeaveMasterDto> GetLeaveMasterById(int id);
        Task<List<LeaveMasterDto>> GetAllLeaveMasters();
        Task<string> AddLeaveMaster(LeaveMasterDto leaveMasterDto);
		Task<string> UpdateLeaveMaster(LeaveMasterDto leaveMasterDto, int id);
		Task<int> DeleteLeaveMaster(int id);
	}
}

using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface IModuleMasterService
    {
        Task<List<ModuleMasterDto>> GetAllModuleMaster();
        Task<ModuleMasterDto> GetModuleMasterById(int moduleId);
		Task<string> AddModuleMaster(ModuleMasterDto moduleMasterDto);
        Task<string> UpdateModuleMaster(ModuleMasterDto moduleMasterDto,int Id);
        Task<int> DeleteModuleMaster(int moduleId);
	}
}

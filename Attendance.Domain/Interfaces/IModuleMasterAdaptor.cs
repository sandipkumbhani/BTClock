using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface IModuleMasterAdaptor
    {
        Task<List<ModuleMasterDto>> GetAllModuleMasterAsync();
		Task<ModuleMasterDto> GetModuleMasterByIdAsync(int moduleId);
		Task<string> AddModuleMasterAsync(ModuleMasterDto moduleMasterDto);
		Task<string> UpdateModuleMasterAsync(ModuleMasterDto moduleMasterDto, int moduleId);
		Task<int> DeleteModuleMasterAsync(int moduleId);
	}
}

using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.service
{
   public  class ModuleMasterService : IModuleMasterService
	{
		private readonly IModuleMasterAdaptor _adaptor;
		public ModuleMasterService(IModuleMasterAdaptor adaptor)
		{
			_adaptor = adaptor;
		}

		public async Task<List<ModuleMasterDto>> GetAllModuleMaster()
		{
			return await _adaptor.GetAllModuleMasterAsync();
		}
		public async Task<ModuleMasterDto> GetModuleMasterById(int Id)
		{
			return await _adaptor.GetModuleMasterByIdAsync(Id);
		}
		public async Task<string> AddModuleMaster(ModuleMasterDto moduleMasterDto)
		{
			return await _adaptor.AddModuleMasterAsync(moduleMasterDto);
		}
		public async Task<string> UpdateModuleMaster(ModuleMasterDto moduleMasterDto, int Id)
		{
			return await _adaptor.UpdateModuleMasterAsync(moduleMasterDto, Id);
		}
		public async Task<int> DeleteModuleMaster(int Id)
		{
			return await _adaptor.DeleteModuleMasterAsync(Id);
		}
	}
}

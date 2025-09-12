using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.service
{
   public class DesignationService:IDesignationService
    {
        private readonly IDesignationAdaptor _designationAdaptor;
		public DesignationService(IDesignationAdaptor designationAdaptor)
		{
			_designationAdaptor = designationAdaptor;
		}
		public async Task<List<DesignationDto>> GetAllDesignation(DesignationDto designationDto)
		{
			return await _designationAdaptor.GetAllDesignationAsync(designationDto);
		}
		public async Task<DesignationDto> GetDesignationById(int designationId)
		{
			return await _designationAdaptor.GetDesignationByIdAsync(designationId);
		}
		public async Task<string> AddDesignation(DesignationDto designationDto)
		{
			return await _designationAdaptor.AddDesignationAsync(designationDto);
		}
		public async Task<string> UpdateDesignation(DesignationDto designationDto, int designationId)
		{
			return await _designationAdaptor.UpdateDesignationAsync(designationDto, designationId);
		}
		public async Task<int> DeleteDesignation(int designationId)
		{
			return await _designationAdaptor.DeleteDesignationAsync(designationId);
		}
	}
}

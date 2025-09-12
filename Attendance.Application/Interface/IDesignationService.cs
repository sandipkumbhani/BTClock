using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface IDesignationService
    {
        Task<DesignationDto> GetDesignationById(int designationId);
        Task<List<DesignationDto>> GetAllDesignation(DesignationDto designationDto);
        Task<string> AddDesignation(DesignationDto designationDto);
		Task<string> UpdateDesignation(DesignationDto designationDto, int designationId);
        Task<int> DeleteDesignation(int designationId);
	}
}

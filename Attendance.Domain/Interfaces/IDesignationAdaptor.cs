using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface IDesignationAdaptor
    {
        Task<DesignationDto> GetDesignationByIdAsync(int designationId);
        Task<List<DesignationDto>> GetAllDesignationAsync(DesignationDto designationDto);
        Task<string> AddDesignationAsync(DesignationDto designationDto);
        Task<string> UpdateDesignationAsync(DesignationDto designationDto, int designationId);
        Task<int> DeleteDesignationAsync(int designationId);
	}
}

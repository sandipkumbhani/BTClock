using Attendance.Domain.Models;

namespace Attendance.Application.Interface
{
    public interface IMenuMasterService
    {
        Task<IEnumerable<menuMasterDto>> GetMenuMasterById(int id);
        Task<menuMasterDto> GetById(int id);
        Task<IEnumerable<menuMasterDto>> GetAllMenuMasters();
        Task<string> AddMenuMaster(menuMasterDto menu);
        Task<string> UpdateMenuMaster(menuMasterDto menu, int id);
        Task<int> DeleteMenuMaster(int id);
    }
}

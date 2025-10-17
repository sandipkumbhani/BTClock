using Attendance.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface IMenuMasterAdaptor
    {
        Task<IEnumerable<MenuMasterDto>> GetAllMenuMastersAsync();
        Task<MenuMasterDto> GetByIdAsync(int id);
        Task<IEnumerable<MenuMasterDto>> GetMenuMasterByIdAsync(int id);
        Task<string> AddMenuMasterAsync(MenuMasterDto menuMasterDto);
        Task<string> UpdateMenuMasterAsync(MenuMasterDto menuMasterDto, int id);
        Task<int> DeleteMenuMasterAsync(int id);
    }
}

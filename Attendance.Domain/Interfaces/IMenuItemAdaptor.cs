using Attendance.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface IMenuItemAdaptor
    {
        Task<IEnumerable<MenuItemDto>> GetAllMenuItemsAsync();
        Task<MenuItemDto> GetMenuItemByIdAsync(int id);
        Task<IEnumerable<MenuItemDto>> GetMenuItemsByMenuIdAsync(int menuId);
        Task<string> AddMenuItemAsync(MenuItemDto menuItemDto);
        Task<string> UpdateMenuItemAsync(MenuItemDto menuItemDto, int id);
        Task<int> DeleteMenuItemAsync(int id);
        Task<IEnumerable<MenuItemDto>> GetAccessibleMenuItemsAsync(int userId);
    }
}

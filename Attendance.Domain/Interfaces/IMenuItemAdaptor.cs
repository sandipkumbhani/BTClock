using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface IMenuItemAdaptor
    {
        Task<IEnumerable<MenuItemDto>> GetAllAsync();
        Task<MenuItemDto> GetByIdAsync(int id);
        Task<IEnumerable<MenuItemDto>> GetByMenuIdAsync(int menuId);
        Task<string> AddMenuItemAsync(MenuItemDto menuItem);
        Task<string> UpdateMenuItemAsync(MenuItemDto menuItem, int id);
        Task<int> DeleteMenuItemAsync(int id);
    }
}

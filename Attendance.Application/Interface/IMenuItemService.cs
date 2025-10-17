using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemDto>> GetAllMenuItems();
        Task<MenuItemDto> GetMenuItemById(int id);
        Task<IEnumerable<MenuItemDto>> GetMenuItemsByMenuId(int menuId);
        Task<string> AddMenuItem(MenuItemDto menuItemDto);
        Task<string> UpdateMenuItem(MenuItemDto menuItemDto, int id);
        Task<int> DeleteMenuItem(int id);
        Task<IEnumerable<MenuItemDto>> GetAccessibleMenuItems(int userId);
    }
}

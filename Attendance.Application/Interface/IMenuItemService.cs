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
        Task<IEnumerable<MenuItemDto>> GetAll();
        Task<MenuItemDto> GetById(int id);
        Task<IEnumerable<MenuItemDto>> GetByMenuId(int menuId);
        Task<string> AddMenuItem(MenuItemDto menuItem);
        Task<string> UpdateMenuItem(MenuItemDto menuItem);
        Task<int> DeleteMenuItem(int id);
    }
}

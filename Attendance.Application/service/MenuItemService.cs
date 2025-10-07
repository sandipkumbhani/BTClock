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
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemAdaptor _menuItemAdaptor;

        public MenuItemService(IMenuItemAdaptor menuItemAdaptor)
        {
            _menuItemAdaptor = menuItemAdaptor;
        }
        public async Task<IEnumerable<MenuItemDto>> GetAll()
        {
            return await _menuItemAdaptor.GetAllAsync();
        }
        public async Task<MenuItemDto> GetById(int id)
        {
            return await _menuItemAdaptor.GetByIdAsync(id);
        }
        public async Task<IEnumerable<MenuItemDto>> GetByMenuId(int menuId)
        {
            return await _menuItemAdaptor.GetByMenuIdAsync(menuId);
        }
        public async Task<string> AddMenuItem(MenuItemDto menuItem)
        {
            return await _menuItemAdaptor.AddMenuItemAsync(menuItem);
        }
        public async Task<string> UpdateMenuItem(MenuItemDto menuItem, int id)
        {
            return await _menuItemAdaptor.UpdateMenuItemAsync(menuItem,id);
        }
        public async Task<int> DeleteMenuItem(int id)
        {
            return await _menuItemAdaptor.DeleteMenuItemAsync(id);
        }
    }
}

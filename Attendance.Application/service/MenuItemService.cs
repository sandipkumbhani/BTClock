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
        public async Task<IEnumerable<MenuItemDto>> GetAllMenuItems()
        {
            return await _menuItemAdaptor.GetAllMenuItemsAsync();
        }
        public async Task<MenuItemDto> GetMenuItemById(int id)
        {
            return await _menuItemAdaptor.GetMenuItemByIdAsync(id);
        }
        public async Task<IEnumerable<MenuItemDto>> GetMenuItemsByMenuId(int menuId)
        {
            return await _menuItemAdaptor.GetMenuItemsByMenuIdAsync(menuId);
        }
        public async Task<string> AddMenuItem(MenuItemDto menuItemDto)
        {
            return await _menuItemAdaptor.AddMenuItemAsync(menuItemDto);
        }
        public async Task<string> UpdateMenuItem(MenuItemDto menuItemDto, int id)
        {
            return await _menuItemAdaptor.UpdateMenuItemAsync(menuItemDto, id);
        }
        public async Task<int> DeleteMenuItem(int id)
        {
            return await _menuItemAdaptor.DeleteMenuItemAsync(id);
        }
        public async Task<IEnumerable<MenuItemDto>> GetAccessibleMenuItems(int userId)
        {
            return await _menuItemAdaptor.GetAccessibleMenuItemsAsync(userId);
        }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Attendance.Domain.Models
{
    public class LoginResponseData
    {
        [JsonProperty("token")]
        public string? Token { get; set; }

        [JsonProperty("user")]
        public UserDto? User { get; set; }

        [JsonProperty("menuItems")]
        public List<MenuItemDto>? MenuItems { get; set; } = new List<MenuItemDto>();
    }

    public class LoginUserDto
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("roleId")]
        public int RoleId { get; set; }

        [JsonProperty("roleName")]
        public string? RoleName { get; set; }
    }

    public class LoginMenuItemDto
    {
        [JsonProperty("menuItemId")]
        public int MenuItemId { get; set; }

        [JsonProperty("menuId")]
        public int MenuId { get; set; }

        [JsonProperty("menuName")]
        public string? MenuName { get; set; }

        [JsonProperty("parentId")]
        public int? ParentId { get; set; }

        [JsonProperty("sortingOrder")]
        public int SortingOrder { get; set; }
    }
}

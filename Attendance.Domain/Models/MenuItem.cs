using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Domain.Models
{
    public class MenuItem
    {
        public MenuItem()
        {
            menuItemDto = new List<MenuItemDto>();
        }
        public List<MenuItemDto> menuItemDto { get; set; }
    }

    public class MenuItemDto
    {
        [Key]
        public int MenuItemId { get; set; }
        public int Menuid { get; set; }
        [ForeignKey("Menuid")]
        public menuMasterDto? MenuMaster { get; set; }
        public string? MenuName { get; set; }
        public int? ParentId { get; set; }
        [RegularExpression("([1-9][0-9]*)")]
        public int SortingOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<MenuItemDto>? Children { get; set; }

    }
}

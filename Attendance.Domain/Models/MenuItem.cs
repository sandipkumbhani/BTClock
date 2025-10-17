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

        [ForeignKey("MenuMasterId")]
        public int MenuId { get; set; }
        public MenuMasterDto? MenuMaster { get; set; }

        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }
        public CompanyDto? Company { get; set; }
        public string MenuName { get; set; }
        public int? ParentId { get; set; }
        public int? SortingOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        [ForeignKey("ParentId")]
        public MenuItemDto Parent { get; set; }
        public ICollection<MenuItemDto> Children { get; set; }

    }
}

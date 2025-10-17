using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Domain.Models
{
    public class UserMenuMapping
    {
        public UserMenuMapping()
        {
            userMenuMappingDto = new List<UserMenuMappingDto>();
        }
        public List<UserMenuMappingDto> userMenuMappingDto { get; set; }
    }
    public class UserMenuMappingDto
    {
        public int UserMenuMappingId { get; set; }

        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }
        public CompanyDto? Company { get; set; }

        [ForeignKey("MenuItemId")]
        public int MenuItemId { get; set; }
        public MenuItemDto? MenuItem { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public UserDto? User { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public List<int>? MenuItemIds { get; set; }

    }
}

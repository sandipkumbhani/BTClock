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
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public int? UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("MenuItemId")]
        public int? MenuItemId { get; set; }
        public MenuItemDto? MenuItem { get; set; }
        public int? InsertBy { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<int>? MenuIds { get; set; }
    }
}

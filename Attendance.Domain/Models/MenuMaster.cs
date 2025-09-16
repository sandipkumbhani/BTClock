    using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Domain.Models
{
    public class MenuMaster
    {
        public MenuMaster()
        {
            menuMasterDto = new List<menuMasterDto>();
        }
        public List<menuMasterDto> menuMasterDto { get; set; }
    }
    public class menuMasterDto
    {
        public int Menuid { get; set; }
        public string Menuname { get; set; }
        public string? MenuPath { get; set; }
        public string? Icon { get; set; }
        public bool isDefault { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey("Id")]
        public int ModuleMasterId { get; set; }
        public ModuleMasterDto? ModuleMaster { get; set; }
        public int? ParentId { get; set; }
        public List<menuMasterDto>? Children { get; set; }
    }
}

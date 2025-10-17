    using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Domain.Models
{
    public class MenuMaster
    {
        public MenuMaster()
        {
            menuMasterDto = new List<MenuMasterDto>();
        }
        public List<MenuMasterDto> menuMasterDto { get; set; }
    }
    public class MenuMasterDto
    {

        public int MenuId { get; set; }

        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }
        public CompanyDto? Company { get; set; }
        public string MenuName { get; set; }
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;

        [ForeignKey("ModuleMasterId")]
        public int ModuleMasterId { get; set; }
        public ModuleMasterDto? ModuleMaster { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

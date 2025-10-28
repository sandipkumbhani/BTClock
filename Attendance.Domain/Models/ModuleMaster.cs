using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
   public class ModuleMaster
    {
        public ModuleMaster() 
        {
			moduleMasterDto = new List<ModuleMasterDto>();
		}
		public List<ModuleMasterDto> moduleMasterDto { get; set; }
	}
	public class ModuleMasterDto
	{
        public int ModuleMasterId { get; set; }

        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }
        public CompanyDto? Company { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

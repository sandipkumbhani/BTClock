using System;
using System.Collections.Generic;
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
		public int Id { get; set; }
		public string ModuleName { get; set; }
	}
}

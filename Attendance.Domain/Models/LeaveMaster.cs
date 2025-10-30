using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
   public class LeaveMaster
    {
        public LeaveMaster()
		{
			leaveMasterDto = new List<LeaveMasterDto>();
		}
		public List<LeaveMasterDto> leaveMasterDto { get; set; }
	}
	public class LeaveMasterDto
	{
		public int LeaveMasterId { get; set; }

		public int CompanyId { get; set; }

		public string Name { get; set; }

		public DateTime? CreatedAt { get; set; }

		public int? CreatedBy { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public int? UpdatedBy { get; set; }
	}
}

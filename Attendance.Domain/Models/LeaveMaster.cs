using System;
using System.Collections.Generic;
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
		public string LeaveType { get; set; }
	}
}

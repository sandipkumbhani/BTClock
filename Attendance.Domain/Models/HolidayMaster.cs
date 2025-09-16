using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
	public class HolidayMaster
	{
		public HolidayMaster()
		{
			holidaymasterDto = new List<HolidayMasterDto>();
		}
		public List<HolidayMasterDto> holidaymasterDto { get; set; }
	}
	public class HolidayMasterDto
	{
		public int HolidayMasterId { get; set; }
		public string? HolidayDate { get; set; }
		public string? HolidayDescription { get; set; }
		public string? Year { get; set; }
		public bool SaveWeekend { get; set; }

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
  public  class Designation
    {
        public Designation() 
        {
			designationDto = new List<DesignationDto>();
		}
        public List<DesignationDto> designationDto { get; set; }
	}
	public class DesignationDto
	{
		public int? DesignationId { get; set; }
		public string DesignationName { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
   public class Department
    {
        public Department()
		{
			departmentDto = new List<DepartmentDto>();
		}
		public List<DepartmentDto> departmentDto { get; set; }
	}
	public class DepartmentDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.Model
{
    public class Department
    {
        public String DepartmentCode { get; set; }
        public String DepartmentName { get; set; }
        public String Status { get; set; }
        public List<String> EmployeeList { get; set; }
    }
}

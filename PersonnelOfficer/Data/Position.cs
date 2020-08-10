using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelOfficer.Data
{
    class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double SalaryFrom { get; set; }
        public double SalaryTo { get; set; }
        public int DepartmentId { get; set; }
    }
}
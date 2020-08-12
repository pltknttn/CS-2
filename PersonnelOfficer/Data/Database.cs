using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PersonnelOfficer.Data
{
    [Serializable]
    public class Database
    {
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Department> Departments { get; set; } = new List<Department>();
        public List<Position> Positions { get; set; } = new List<Position>();
        [XmlIgnore]
        public int NextEmployeId => (Employees?.Any() == true ? Employees.Max(x => x.Id) : 0) + 1;
        [XmlIgnore]
        public int NextDepartmentId => (Departments?.Any() == true ? Departments.Max(x => x.Id) : 0) + 1;
        [XmlIgnore]
        public int NextPositionId => (Positions?.Any() == true ? Positions.Max(x => x.Id) : 0) + 1;
    }
}

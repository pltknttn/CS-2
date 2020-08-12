using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PersonnelOfficer.Data
{
    public class Employee: ICloneable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-30);
        [XmlIgnore]
        public int Age => UtilClass.GetYearDiff(DateOfBirth, DateTime.Now);
        public double Salary { get; set; } 
        public string Address { get; set; } 
        public string Telephone { get; set; }
        public string Email { get; set; }
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{Surname} {FirstName} {Patronymic}";
        }
    }  
}

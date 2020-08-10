using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelOfficer.Data
{
    class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; } = Sex.Man;
        public bool Married { get; set; } = false;
        public int Age => UtilClass.GetYearDiff(DateOfBirth, DateTime.Now);
        public double Salary { get; set; }
        public string INN { get; set; }
        public string Address { get; set; } 
        public string Telephone { get; set; }
        public string Email { get; set; }
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }
        public byte[] Photo { get; set; }
        public DateTime EmploymentDate { get; set; }
        public DateTime? DismissalDate { get; set; }
    }  
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelOfficer.Data
{
    public sealed class Position: ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double SalaryFrom { get; set; }
        public double SalaryTo { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
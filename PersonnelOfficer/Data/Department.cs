using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelOfficer.Data
{
    public class Department:ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string RommNumber { get; set; }

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

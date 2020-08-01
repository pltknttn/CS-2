using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2.Data
{
    class FixedPayEmployee: BaseEmployee
    {
        public FixedPayEmployee() { }

        public FixedPayEmployee(string fio, string position, double fixedSalary) : base(fio, position)
        {
            FixedSalary = fixedSalary;
        }

        public double FixedSalary { get; set; }

        public override object Clone()
        {
            return new FixedPayEmployee(this.FIO, this.Position, this.FixedSalary);
        }

        public override bool Equals(BaseEmployee other)
        {
            if (other is FixedPayEmployee obj)
            {
                return (obj.FIO == this.FIO && obj.Position == this.Position && obj.FixedSalary == this.FixedSalary);
            }
            return false;
        }

        public override double GetAvgSalary()
        {
            return FixedSalary;
        }
    }
}

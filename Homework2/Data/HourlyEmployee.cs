using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2.Data
{
    class HourlyEmployee : BaseEmployee
    {
        public HourlyEmployee() : this(null, null, 0, 8) { }
        
        public HourlyEmployee(string fio, string position, double hourlyRate) : this(fio, position, hourlyRate, 8)
        {
        }

        public HourlyEmployee(string fio, string position, double hourlyRate, int hoursWorked) :base(fio, position)
        {
            HourlyRate = hourlyRate;
            HoursWorked = hoursWorked;
        }

        public int HoursWorked { get; set; }

        public double HourlyRate { get; set; }

        public override object Clone()
        {
            return new HourlyEmployee(this.FIO, this.Position, this.HourlyRate, this.HoursWorked);
        }

        public override bool Equals(BaseEmployee other)
        {
            if (other is HourlyEmployee obj)
            {
                return (obj.FIO == this.FIO && obj.Position == this.Position && obj.HoursWorked == this.HoursWorked && obj.HourlyRate == this.HourlyRate);
            }
            return false;
        }

        public override double GetAvgSalary()
        {
            return 20.8 * HourlyRate * HourlyRate;
        }
    }
}

using System;
using System.Runtime;

namespace Homework2.Data
{
    abstract class BaseEmployee : IComparable, ICloneable, IEquatable<BaseEmployee>
    {
        public BaseEmployee() { }
        
        public BaseEmployee(string fio, string position): base()
        {
            FIO = fio;
            Position = position;
        }

        public string FIO { get; set; }
        public string Position { get; set; }
                
        public virtual int CompareTo(object obj)
        {
            double currentAvgSalary = this.GetAvgSalary();
            double compareAvgSalary = ((BaseEmployee)obj)?.GetAvgSalary() ?? 0.0;
            return currentAvgSalary < compareAvgSalary ? -1 : currentAvgSalary > compareAvgSalary ? 1 : 0;
        }

        public abstract double GetAvgSalary();
        public abstract object Clone();
        public abstract bool Equals(BaseEmployee other);

        public override string ToString()
        {
            return $"{FIO} {GetAvgSalary()}";
        }
    }
}

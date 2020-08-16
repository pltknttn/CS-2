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
        /// <summary>Должность</summary>
        public string Name { get; set; }
        /// <summary>Оклад (от)</summary>
        public double SalaryFrom { get; set; } = 0.0D;
        /// <summary>Оклад (до)</summary>
        public double SalaryTo { get; set; } = 0.0D;
        /// <summary>Отдел</summary>
        public int DepartmentId { get; set; }
        /// <summary>Последнее состояние</summary>
        public EditState LastEditState { get; set; } = EditState.Insert;

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
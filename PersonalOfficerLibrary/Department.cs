using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalOfficerLibrary
{
    public class Department:ICloneable
    {
        public int Id { get; set; }
        /// <summary>Отдел</summary>
        public string Name { get; set; }
        /// <summary>Номер вн. телефона</summary>
        public string Telephone { get; set; }
        /// <summary>Расположение (адрес)</summary>
        public string Address { get; set; }
        /// <summary>Номер комнаты\помещения</summary>
        public string RommNumber { get; set; }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PersonalOfficerLibrary
{
    public class Employee: ICloneable
    {
        public int Id { get; set; }
        /// <summary>Имя</summary>
        public string FirstName { get; set; }
        /// <summary>Отчество</summary>
        public string Patronymic { get; set; }
        /// <summary>Фамилия/summary>
        public string Surname { get; set; }
        /// <summary>Обращение</summary>
        public string Title { get; set; }
        /// <summary>Оклад</summary>
        public double Salary { get; set; } = 0.0D;
        /// <summary>Дата рождения</summary>
        public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-20).Date;
        public Sex Sex { get; set; } = Sex.Female;
        /// <summary>В браке</summary>
        public bool Married { get; set; }
        /// <summary>Адрес</summary>
        public string Address { get; set; }
        /// <summary>Вн. телефон</summary>
        public string Telephone { get; set; }
        /// <summary>Мобильный телефон</summary>
        public string Mobilephone { get; set; }
        /// <summary>Почта</summary>
        public string Email { get; set; }
        /// <summary>Должность</summary>
        public int PositionId { get; set; }
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
            return $"{Surname} {FirstName} {Patronymic}";
        }
    }  
}

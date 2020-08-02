using Homework2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2
{
    class Program
    {
        static void Main()
        {
            /*Домашняя работа 2. 
             1. Построить три класса (базовый и 2 потомка), описывающих работников с почасовой
оплатой (один из потомков) и фиксированной оплатой (второй потомок):
a. Описать в базовом классе абстрактный метод для расчета среднемесячной
заработной платы. Для «повременщиков» формула для расчета такова:
«среднемесячная заработная плата = 20.8 * 8 * почасовая ставка»; для работников с
фиксированной оплатой: «среднемесячная заработная плата = фиксированная
месячная оплата»;
b. Создать на базе абстрактного класса массив сотрудников и заполнить его;
c. * Реализовать интерфейсы для возможности сортировки массива, используя
Array.Sort();
d. * Создать класс, содержащий массив сотрудников, и реализовать возможность вывода
данных с использованием foreach.

             */

            Console.WriteLine("--Массив сотрудников--");

            BaseEmployee[] list1 = {
                new FixedPayEmployee("Маркелов Иван Иванович", "Управляющий", 25000),
                new HourlyEmployee("Маряхин Петр Петрович", "Сантехник", 1000, 4),
                new FixedPayEmployee("Ларина Ольга Васильевна", "Директор", 40000),
                new HourlyEmployee("Филиппова Валерия Ивановна", "Инженер", 2000, 6),
                new FixedPayEmployee("Логинов Иван Иванович", "Заместитель директора", 30000)};

            foreach (var obj in list1) Console.WriteLine(obj);
            
            Console.WriteLine("--Sort--");
            Array.Sort(list1);


            Console.WriteLine("--Массив сотрудников через объект список сотрудников (foreach)--");

            EmployeeList list = new EmployeeList(10)
            {
                new FixedPayEmployee("Маркелов Иван Иванович", "Управляющий", 25000),
                new HourlyEmployee("Маряхин Петр Петрович", "Сантехник", 1000, 4),
                new FixedPayEmployee("Ларина Ольга Васильевна", "Директор", 40000),
                new HourlyEmployee("Филиппова Валерия Ивановна", "Инженер", 2000, 6),
                new FixedPayEmployee("Логинов Иван Иванович", "Заместитель директора", 30000),
                new HourlyEmployee("Кожадей Леонид Витальевич", "Дворник", 1000, 4),
                new FixedPayEmployee("Захарова Лариса Евгеньевна","Бухгалтер", 30000),
                new HourlyEmployee("Федюнина Елизавета Олеговна", "Уборщица", 1000, 3),
                new HourlyEmployee("Маркелов Геннадий Алексеевич","Электрик", 2000, 3),
                new FixedPayEmployee("Филин Георгий Сергеевич", "Кадровик", 20000)
            }; 

            foreach (var obj in list)  Console.WriteLine(obj);

            Console.WriteLine("--Sort--");
            list.Sort();
            foreach (var obj in list) Console.WriteLine(obj);

            Console.WriteLine("Для выхода нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}

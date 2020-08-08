using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson4
{
    class Program
    {
        /* Домашняя работа 4.
         *  2. Дана коллекция ​ List<T>​ . Требуется подсчитать, сколько раз каждый элемент встречается в  данной коллекции: 
                    a. для целых чисел;  
                    b. * для обобщенной коллекции;  
                    c. ** используя Linq. 
            3. * Дан фрагмент программы.
                    а. Свернуть обращение к ​ OrderBy​ с использованием лямбда-выражения​ =>. 
                    b. * Развернуть обращение к ​ OrderBy​ с использованием делегата . 
           Автор: Полятыкина Татьяна
         */

        static Random _random = new Random();
        static void Main()
        {
            List<int> listInts = new List<int>();
            for(int i = 0; i < 100; i++)
            {
                listInts.Add(_random.Next(1, 100));                
            }

            Console.WriteLine($"Коллекция целых чисел: {string.Join("; ", listInts)}");

            int countElements = listInts.Count;
            Console.WriteLine($"Количество элементов = {countElements}");

            countElements = Counter<int>.CountElements(listInts, (x) => x % 2 == 0);
            Console.WriteLine($"Количество четных элементов = {countElements}");

            Console.WriteLine($"\n\rВариант посчитать через Dictionary:");
            Dictionary<int, int> itemCounts = new Dictionary<int, int>();
            foreach (var i in listInts)
            {
                if (!itemCounts.ContainsKey(i)) itemCounts.Add(i, 0);
                itemCounts[i]++;
            }
            foreach (var item in itemCounts)
            {
                Console.WriteLine($"Элемент {item.Key} встречается {item.Value} раз.");
            }

            Console.WriteLine($"\n\rВариант посчитать через обобщенный статический метод:");
            foreach (var i in listInts.Distinct())
            { 
                countElements = Counter<int>.CountElements(listInts, i, (x, y) => x == y);
                Console.WriteLine($"Элемент {i} встречается {countElements} раз.");
            }


            Console.WriteLine($"\n\rВариант посчитать используя linq:");
            foreach (var item in listInts.Distinct().Select(x=> new { Item = x, Counts = listInts.Count(y=>y==x)}))
            {
                Console.WriteLine($"Элемент {item.Item} встречается {item.Counts} раз.");
            }

            Console.WriteLine("\n\rДля перехода к следующему заданию нажмите любую клавишу...");
            Console.ReadKey();

            Console.Clear();
            Dictionary<string, int> dict = new Dictionary<string, int>()
            {
                { "four", 4},
                { "two", 2},
                { "one", 1},
                { "three", 3}
            };

            Console.WriteLine("Дан словарь (Dictionary) с элементами:");
            foreach (var pair in dict)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }

            Console.WriteLine("\n\rСортировка с delegate:");
            var d = dict.OrderBy(delegate (KeyValuePair<string, int> pair) { return pair.Value;  });
            foreach(var pair in d)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }

            Console.WriteLine("\n\rСортировка с использованием лямбда-выражений:");
            d = dict.OrderBy((pair) => pair.Value);
            foreach (var pair in d)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }

            Console.WriteLine("\n\rДля выхода нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}

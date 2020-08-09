using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson4
{
    static class ExampleExtensions
    {
        /// <summary>Добавление элементов в коллекцию</summary>     ​ 
        /// <typeparam name="T"></typeparam>     ​ 
        /// <param name="self"></param>     ​ 
        /// <param name="coll"></param>     ​ 
        /// <returns></returns>     ​ 
        public static T AddTo<T>(this T self, ICollection<T> coll)
        {
            coll.Add(self);
            return self;
        }
        /// <summary>Проверяет, существует ли элемент в коллекции</summary>     ​ 
        /// <typeparam name="T"></typeparam>     ​ 
        /// <param name="self"></param>     ​ 
        /// <param name="elem"></param>     ​
        /// <returns></returns>     
        public static bool IsOneOf<T>(this T self, ICollection<T> coll)
        {
            return coll.Contains(self);
        }
        /// <summary>
        /// Считает сколько раз элемент встречается в коллекции
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="coll"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static int CountElements<T>(this T self, List<T> coll)
        {
            if (coll == null || coll.Count == 0 || self == null) return 0;

            //int count = 0;
            //foreach (var element in coll) if (element.Equals(self)) count++;
            //return count;
            return coll.Count(element => element.Equals(self));            
        } 
        /// <summary>
        /// Cчитает сколько раз втсречается элемент в коллекции при условии
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="coll"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static int CountElements<T>(this T self, List<T> coll, Func<T, T, bool> fun)
        {
            if (coll == null || coll.Count == 0 || self == null) return 0;
            if (fun == null) return self.CountElements(coll);

            //int count = 0;
            //foreach (var element in coll) if (fun.Invoke(element, self)) count++;
            //return count;

            return coll.Count(element => fun.Invoke(element, self));
        }
    }
}

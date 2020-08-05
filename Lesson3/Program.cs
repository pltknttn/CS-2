using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson3
{
    /*Задание: 5. * Добавить в пример Lesson3 обобщенный делегат. */
    
    /// <summary>
    /// Обобщенный делегат
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o"></param>
    public delegate void MyDelegate<T>(T o);

    /// <summary>
    /// Обобщенный класс
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Source<T>
    {
        public event MyDelegate<T> Run;

        /// <summary>
        /// Объект T стартовал 
        /// </summary>
        public void Start(T obj)
        {
            Console.WriteLine("RUN");              

            Run?.Invoke(obj);
        }
    }

    /// <summary>
    /// Интерфейс наблюдатель
    /// </summary>
    interface IObserver
    {
        string Name { get;}
        void Do(IObserver o);
    }

    /// <summary>
    /// 1 наблюдатель
    /// </summary>
    class Observer1 : IObserver
    {
        public string Name { get => "Observer1"; }

        public void Do(IObserver o)
        {
            Console.WriteLine("Первый. Принял, что объект {0} побежал", o?.Name);  
        } 
    }

    /// <summary>
    /// 2 наблюдатель
    /// </summary>
    class Observer2 : IObserver
    {
        public string Name { get => "Observer2"; }

        public void Do(IObserver o)
        {
            Console.WriteLine("Второй. Принял, что объект {0} побежал", o?.Name);
        } 
    }

    class Program
    {
        static void Main()
        {
            Source<IObserver> s = new Source<IObserver>();
            Observer1 o1 = new Observer1();
            Observer2 o2 = new Observer2();
            MyDelegate<IObserver> d = new MyDelegate<IObserver>(o1.Do);
            s.Run += d;
            s.Start(o2);
            s.Run += o2.Do;
            s.Start(o2);
            s.Run -= d;
            s.Start(o1);
            Console.ReadKey();
        }
    }
}

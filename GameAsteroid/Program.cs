using GameAsteroid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAsteroid
{
    static class Program
    {
        /* Домашняя работа 3.
         * 1. Добавить космический корабль, как описано в уроке.
           2. Доработать игру «Астероиды»:
               a. Добавить ведение журнала в консоль с помощью делегатов;
               b. * добавить это и в файл.
           3. Разработать аптечки, которые добавляют энергию.
           4. Добавить подсчет очков за сбитые астероиды.
           
           5. * Добавить в пример Lesson3 обобщенный делегат. 

         *  Домашняя работа 2. 
          2. Переделать виртуальный метод ​ Update в ​ BaseObject в абстрактный и реализовать его в            
          наследниках. 
          3. Сделать так, чтобы при столкновении пули с астероидом они регенерировались в разных            
          концах экрана. 
          4. Сделать проверку на задание размера экрана в классе ​ Game​ . Если высота или ширина             
          (Width, Height) больше 1000 или принимает отрицательное значение, выбросить исключение   
           ArgumentOutOfRangeException()​ .
          5. * Создать собственное исключение ​ GameObjectException​ , которое появляется при попытке         
           создать объект с неправильными характеристиками (например, отрицательные размеры,        
           слишком большая скорость или неверная позиция). 
         */
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Form
            {
                Width = Screen.PrimaryScreen.Bounds.Width - 100,
                Height = Screen.PrimaryScreen.Bounds.Height - 100,
                Text = "Домашняя работа 3. Автор: Полятыкина Татьяна",
                StartPosition = FormStartPosition.CenterScreen
            }; 
                       
            Game.Init(form);
            form.Show();            
            Game.Draw();

            Application.Run(form);
        }   
        
    }
}

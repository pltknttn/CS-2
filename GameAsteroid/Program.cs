using GameAsteroid.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAsteroid
{
    static class Program
    {
        /* Домашняя работа 4.
         *  1. Добавить в программу коллекцию астероидов. Как только она заканчивается (все астероиды  сбиты), формируется новая коллекция, в которой на один астероид больше. 
            2. Дана коллекция ​ List<T>​ . Требуется подсчитать, сколько раз каждый элемент встречается в  данной коллекции: 
                    a. для целых чисел;  
                    b. * для обобщенной коллекции;  
                    c. ** используя Linq. 
            3. * Дан фрагмент программы.
                    а. Свернуть обращение к ​ OrderBy​ с использованием лямбда-выражения​ =>. 
                    b. * Развернуть обращение к ​ OrderBy​ с использованием делегата . 
           Автор: Полятыкина Татьяна
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
                Width = Screen.PrimaryScreen.Bounds.Width - 300,
                Height = Screen.PrimaryScreen.Bounds.Height - 300,
                Text = "Домашняя работа 4. Автор: Полятыкина Татьяна",
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new Size(500,600),
                Icon = GameResources.GameIco
            }; 
                       
            Game.Init(form);
            form.Show();            
            Game.Draw();

            Application.Run(form);
        }   
        
    }
}

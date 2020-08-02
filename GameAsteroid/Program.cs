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
        /*
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

            Form form = new Form
            {
                Width = Screen.PrimaryScreen.Bounds.Width,
                Height = Screen.PrimaryScreen.Bounds.Height
            };
            form.Text = "Домашняя работа 2. Автор: Полятыкина Татьяна";
            form.SizeChanged += Form_SizeChanged;

            Game.Init(form);
            form.Show();            
            Game.Draw();

            Application.Run(form);
        }

        private static void Form_SizeChanged(object sender, EventArgs e)
        {
            Form form = (Form)sender;
            Game.SizeChanged(form);
            Game.Draw();
        }
    }
}

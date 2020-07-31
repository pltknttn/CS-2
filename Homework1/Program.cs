using Homework1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework1
{
    static class Program
    {
        /// <summary>
        /// Домашняя работа 1. 
        /// 1. Добавить свои объекты в иерархию объектов, чтобы получился красивый задний фон,
        ///похожий на полет в звездном пространстве.
        ///2. * Заменить кружочки картинками, используя метод DrawImage.
        ///Выполнила: Полятыкина Татьяна
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form form = new Form
            {
                Width = 800,
                Height = 600
            };
            form.Text = "Домашняя работа 1. Автор: Полятыкина Татьяна";
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

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

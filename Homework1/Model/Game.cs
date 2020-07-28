using Homework1.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework1.Model
{
    static class Game
    {
        private static BufferedGraphicsContext _context; 
        private static BaseObject[] _objs;
        private static Timer _timer;

        public static Random Random { get; private set; }
        public static BufferedGraphics Buffer { get; private set; }

        //Ширина игрового поля 
        public static int Width { get; private set; }

        //Высота игрового поля 
        public static int Height { get; private set; }

        static Game() { }

        public static void Init(Form form)
        {
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;

            // Графическое устройство для вывода графики
            Graphics g = form.CreateGraphics();

            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            Random = new Random();

            Load();

            _timer = new Timer { Interval = 100 };
            _timer.Start();
            _timer.Tick += Timer_Tick;
        }

        public static void SizeChanged(Form form)
        {
            if (_context == null)
            {
                Init(form);
                return;
            }

            _timer.Enabled = false;
            
            // Графическое устройство для вывода графики
            Graphics g = form.CreateGraphics();

            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            _timer.Enabled = true;
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        public static void Draw()
        {
            DrawObjects();
        }

        private static void DrawTest()
        {
            //Проверяем вывод графики
            Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
            Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
            Buffer.Render();
        }

        private static void DrawObjects()
        {            
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs) obj.Draw();
            Buffer.Render();
        }

        private static void LoadBaseObjects(int a, int b, int minSize = 5, int maxSize = 20)
        {
            int sizeObj;
            for (int i = a; i < b; i++)
            {
                sizeObj = Random.Next(minSize, maxSize + 1);
                _objs[i] = new BaseObject(new Point(600, i * 20), new Point(15 - i, 15 - i), new Size(sizeObj, sizeObj));
            }
        }

        private static void LoadStars(int a, int b, int minSize = 5, int maxSize = 20)
        {
            int sizeObj; 
            for (int i = a; i < b; i++)
            {
                sizeObj = Random.Next(minSize, maxSize + 1);
                _objs[i] = new Star(new Point(600, i * 20), new Point(-i, 0), new Size(sizeObj, sizeObj));
            }
        }

        private static void LoadRhombus(int a, int b, int minSize = 5, int maxSize = 20)
        {
            int sizeObj;
            for (int i = a; i < b; i++)
            {
                sizeObj = Random.Next(minSize, maxSize + 1);
                _objs[i] = new Rhombus(new Point(600, i * 20), new Point(-i, 5-i), new Size(sizeObj, sizeObj));
            }
        }

        private static void LoadAsteroid(int a, int b, int minSize = 5, int maxSize = 20)
        {
            int sizeObj;
            for (int i = a; i < b; i++)
            {
                sizeObj = Random.Next(minSize, maxSize + 1);
                _objs[i] = new Asteroid(new Point(100, i * 5), new Point( 5 - i, 0), new Size(sizeObj, sizeObj));
            }
        }

        private static void LoadRocket(int a, int b, int minSize = 5, int maxSize = 20)
        {
            int sizeObj;
            for (int i = a; i < b; i++)
            {
                sizeObj = Random.Next(minSize, maxSize + 1);
                _objs[i] = new Rocket(new Point(Random.Next(100, 300), i * Random.Next(3, 10)), new Point(0, 0), new Size(sizeObj, sizeObj));
            }
        }

        private static void LoadSpacecraft(int a, int b, int minSize = 5, int maxSize = 20)
        {
            int sizeObj;
            for (int i = a; i < b; i++)
            {
                sizeObj = Random.Next(minSize, maxSize + 1);
                _objs[i] = new Spacecraft(new Point(300, i * 10), new Point(5-i, 0), new Size(sizeObj, sizeObj));
            }
        }

        private static void LoadPlanet(int a, int b, string resourceName, int minSize = 5, int maxSize = 20)
        {
            Bitmap img = new Bitmap(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName));
            int sizeObj;
            for (int i = a; i < b; i++)
            {
                sizeObj = Random.Next(minSize, maxSize + 1);
                _objs[i] = new Planet(new Point(Random.Next(10, Game.Width) - i * 2, Random.Next(50, Game.Height) - i), new Point(10-i, 0), new Size(sizeObj, sizeObj), img);
            }
        }


        private static void Load()
        {
            _objs = new BaseObject[35];

            int objLength = _objs.Length - 5;

            LoadBaseObjects(0, objLength / 3, 5, 10); 
            LoadStars(objLength / 3, 2 * objLength / 3, 4, 6);
            LoadRhombus(2 * objLength / 3, objLength, 5, 8);             
            LoadRocket(objLength, objLength + 1, 2, 2);
            LoadPlanet(objLength + 1, objLength + 2, "Homework1.Images.planet_40px.png", 2, 2);
            LoadPlanet(objLength + 2, objLength + 3, "Homework1.Images.venus_planet_48px.png", 2, 2);
            LoadAsteroid(objLength + 3, objLength + 4, 2, 2);
            LoadSpacecraft(objLength + 4, _objs.Length, 2, 2);
        }

        private static void Update()
        {
            foreach (BaseObject obj in _objs) obj.Update();
        }

    }
}

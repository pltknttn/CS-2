using GameAsteroid.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAsteroid.Model
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        private static BaseObject[] _objs;
        private static Bullet _bullet;
        private static Asteroid[] _asteroids;
        private static Timer _timer;
        private static int MaxWidth = Screen.PrimaryScreen.Bounds.Width;
        private static int MaxHeight = Screen.PrimaryScreen.Bounds.Height;

        public static Random Random { get; private set; }
        public static BufferedGraphics Buffer { get; private set; }

        //Ширина игрового поля 
        private static int _width;
        public static int Width
        {
            get { return _width; }
            private set
            {
                if (value >= 0 && value <= MaxWidth) _width = value;
                else throw new ArgumentOutOfRangeException("Некорректная ширина игрового поля");
            }
        }

        //Высота игрового поля 
        private static int _height;
        public static int Height
        {
            get { return _height; }
            private set
            {
                if (value >= 0 && value <= MaxHeight) _height = value;
                else throw new ArgumentOutOfRangeException("Некорректная высота игрового поля");
            }
        }

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
         
        private static void DrawObjects()
        {
            Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(50, 50, 100, 100));
            foreach (BaseObject obj in _objs) obj?.Draw();
            foreach (Asteroid obj in _asteroids) obj?.Draw();
            _bullet?.Draw();
            Buffer.Render();
        }
                          

        private static void Load()
        {
            _objs = new BaseObject[30];
            _bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(8, 4));
            _asteroids = new Asteroid[3];
            
            for (int i = 0; i < _objs.Length; i++)
            {
                int r = Random.Next(5, 50);
                _objs[i] = new Star(new Point(1000, Random.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            }

            for(int i = 0; i < _asteroids.Length; i++)
            {
                int r = Random.Next(5, 50);
                _asteroids[i] = new Asteroid(new Point(1000, Random.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r));
            } 
        }

        private static void Update()
        {
            foreach (BaseObject obj in _objs) obj?.Update();
            foreach (Asteroid obj in _asteroids)
            {
                if (obj == null) continue;

                obj.Update();

                if (obj.Collision(_bullet))
                {
                    System.Media.SystemSounds.Hand.Play();

                    obj.Move(new Point(0, Random.Next(0, Game.Height)));
                    _bullet.Move(new Point(Game.Height, Random.Next(0, Game.Height)));
                }
            }
            _bullet?.Update();
        }

    }
}

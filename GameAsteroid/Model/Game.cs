using GameAsteroid.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text; 
using System.Windows.Forms;

namespace GameAsteroid.Model
{
    static class GameResources
    {
        public static Image ShipImage = Image.FromFile(@"Images\space_shuttle.png");
        public static Image AsteroidImage = Image.FromFile(@"Images\asteroid.png");
        public static Image MedKitImage = Image.FromFile(@"Images\doctors_bag.png");        
    }

    static class Game
    {
        public const int MAX_ENERGY = 100;

        private static BufferedGraphicsContext _context;
        private static BaseObject[] _objs;
        private static Bullet[] _bullets;
        private static Asteroid[] _asteroids;
        private static MedicineKit[] _medKits;
        private static Ship _ship;
        private static Timer _timer;
        private static int MaxWidth = Screen.PrimaryScreen.Bounds.Width;
        private static int MaxHeight = Screen.PrimaryScreen.Bounds.Height;
        private static Font _font = new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline);
        private static Rectangle RectSun = new Rectangle(50, 50, 100, 100);
        private static Size _medKitSize = new Size(24, 24);
        private static Point _nilDir = new Point(0, 0);
        private static Form _form;

        public static Random Random { get; private set; }
        public static BufferedGraphics Buffer { get; private set; }

        /// <summary>
        /// Запись в лог
        /// </summary>
        public static Action<string> ActionWriteLog;

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

        static Game()
        {
            ActionWriteLog += new Action<string>(GameLog.Instance.WriteFile);
            ActionWriteLog += new Action<string>(GameLog.Instance.WriteConsole);
        }

        public static void Init(Form form)
        {
            ActionWriteLog?.Invoke("Game: Init");

            _form = form; 

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

            Ship.MessageDie += Finish;

            _timer = new Timer { Interval = 100 };
            _timer.Start();
            _timer.Tick += Timer_Tick;

            form.SizeChanged += Form_SizeChanged;
            form.KeyDown += Form_KeyDown; 
        }

        private static void Form_SizeChanged(object sender, EventArgs e)
        { 
            _timer.Enabled = false;

            // Графическое устройство для вывода графики
            Graphics g = _form.CreateGraphics();

            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = _form.ClientSize.Width;
            Height = _form.ClientSize.Height;

            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            _timer.Enabled = true;

            Game.Draw();
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            lock (_ship)
            {
                switch (e.KeyCode)
                {
                    case Keys.ControlKey:
                        {
                            if (_ship == null) break;
                            int move = Random.Next(0, 2); 
                            for (int j = 0; j < _bullets.Length; j++)
                            { 
                                _bullets[j] = new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(4, 0), new Size(_ship.Rect.Width / 6, _ship.Rect.Height / 12), move, 5);
                                _ship.Strike(1);
                            }
                            break;
                        }
                    case Keys.Up: _ship.Up(); break;
                    case Keys.Down: _ship.Down(); break;
                    case Keys.Left: _ship.Left(); break;
                    case Keys.Right: _ship.Right(); break;
                }
            }
        }
                
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.FillEllipse(Brushes.Wheat, RectSun);
            foreach (BaseObject obj in _objs) obj?.Draw();
            foreach (Asteroid obj in _asteroids) obj?.Draw();
            foreach (BaseObject obj in _bullets) obj?.Draw();
            foreach (BaseObject obj in _medKits) obj?.Draw();
            if (_ship != null)
            {
                _ship.Draw();
                Buffer.Graphics.DrawString($"Energy: {_ship.Energy}", SystemFonts.DefaultFont, Brushes.White, 0, 3);
                Buffer.Graphics.DrawString($"Shoots: {_ship.ShootsCounter}", SystemFonts.DefaultFont, Brushes.White, 0, 17);
                Buffer.Graphics.DrawString($"Hit: {_ship.HitCounter}", SystemFonts.DefaultFont, Brushes.White, 0, 31);
            }
            Buffer.Render();            
        }
         
        private static void Finish()
        {
            _timer.Stop();
            ActionWriteLog?.Invoke("Game: Finish");
            Buffer.Graphics.DrawString($"The End", _font, Brushes.White, 200, 100); 
            Buffer.Render();            
        }
                                
        private static void Load()
        {
            ActionWriteLog?.Invoke("Game: Load");

            _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(36, 36));            
            _objs = new BaseObject[30];
            _asteroids = new Asteroid[Random.Next(4, 16)];
            _bullets = new Bullet[Random.Next(3, 20)];
            _medKits = new MedicineKit[Random.Next(1, 10)];
            for (int i = 0; i < _objs.Length; i++)
            {
                int r = Random.Next(5, 50);
                _objs[i] = new Star(new Point(1000, Random.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            }
            for(int i = 0; i < _asteroids.Length; i++)
            {
                int r = Random.Next(10, 55);
                _asteroids[i] = new Asteroid(new Point(Random.Next(0, Game.Width), Random.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r));
            }
            ActionWriteLog?.Invoke($"Game: Objs = {_objs.Length}, Asteroids = {_asteroids.Length}, Bullets = {_bullets.Length}");
        }

        private static void Update()
        {
            foreach (BaseObject obj in _objs) obj?.Update();
            for (int i = 0; i < _asteroids.Length; i++)
            {
               if (_asteroids[i] == null) continue;

                _asteroids[i].Update();

                bool bulletHitTheMark = false;
                for (int j = 0; j < _bullets.Length; j++)
                { 
                    if (_bullets[j] != null && _bullets[j].Collision(_asteroids[i]))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        bulletHitTheMark = true;
                        _bullets[j] = null;
                        _ship?.Slay(1);
                    }
                }
                if (bulletHitTheMark)
                {
                    _asteroids[i] = null;
                    continue;
                }
                if (_ship == null) continue;
                for (int j = 0; j < _medKits.Length; j++)
                {
                    if (_medKits[j] != null && _asteroids[i].Collision(_medKits[j]))
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        _medKits[j] = null;
                        continue;
                    }
                    if (_medKits[j] != null && _ship.Collision(_medKits[j]))
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        _ship.EnergyHigh(_medKits[j].Energy);
                        _medKits[j] = null;
                        continue;
                    }                   
                }
                if (!_ship.Collision(_asteroids[i])) continue;

                _ship.EnergyLow(Random.Next(1, 10));

                System.Media.SystemSounds.Asterisk.Play();

                if (_ship.Energy <= 0) _ship.Die();
                else _ship.Move(new Point(Random.Next(0, Game.Width), Random.Next(0, Game.Height)));  
            }
            foreach (BaseObject obj in _bullets) obj?.Update();
            if (_ship != null && _ship.Energy < Game.MAX_ENERGY)
            {
                for (int j = 0; j < _medKits.Length; j++)
                {
                    if (_medKits[j] != null) continue;
                    _medKits[j] = new MedicineKit(new Point(Random.Next(_ship.Rect.X + Random.Next(10, 50), _ship.Rect.X + Random.Next(100, 500)), Random.Next(_ship.Rect.Y + Random.Next(10, 40), _ship.Rect.Y + Random.Next(100, 400))), _nilDir, _medKitSize, Random.Next(5, Game.MAX_ENERGY));
                }
            }
        }

    }
}

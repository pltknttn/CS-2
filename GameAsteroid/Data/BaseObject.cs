using GameAsteroid.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroid.Data
{
    abstract class BaseObject: ICollision
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size; 

        public BaseObject(Point pos) : this(pos, new Size(5, 5)) { }

        public BaseObject(Point pos, Size size) : this(pos, new Point(0, 0), size) { }

        public BaseObject(Point pos, Point dir, Size size)
        {
            if (pos == null) throw new GameObjectException("Позиция объекта не задана.");

            if (pos.X < 0 || pos.X > Game.Width)
                throw new GameObjectException("Объект за пределами видмости ( некорректная координата X).");

            if (pos.Y < 0 || pos.Y > Game.Height)
                throw new GameObjectException("Объект за пределами видмости ( некорректная координата Y).");

            if (size == null) throw new GameObjectException("Размер объекта не задан.");

            if (size.Height <= 0 || size.Height > Game.Height) throw new GameObjectException($"Некорректный размер объекта (высота объекта больше 0 и меньше {Game.Height}).");
            if (size.Width <= 0 || size.Width > Game.Width) throw new GameObjectException($"Некорректный размер объекта (ширина объекта больше 0 и меньше {Game.Width}.");

            Pos = pos;

            /*Если позиция объекта за пределами области, то вернем в область видимости*/
            //if (Pos.X < 0) Pos.X += Game.Random.Next(10, Game.Width);
            //if (Pos.X > Game.Width) Pos.X = Game.Random.Next(Game.Width / 4, Game.Width / 2 + 1);

            //if (Pos.Y < 0) Pos.Y += Game.Random.Next(10, Game.Height);
            //if (Pos.Y > Game.Height) Pos.Y = Game.Random.Next(Game.Height / 4, Game.Height / 2 + 1);

            Dir = dir;
            Size = size;
        }

        public Rectangle Rect => new Rectangle(Pos, Size);

        public bool Collision(ICollision obj) => obj?.Rect.IntersectsWith(this.Rect)??false; 

        public abstract void Draw();

        public abstract void Update();

        /// <summary>
        /// Переместить объект в позицию
        /// </summary>
        /// <param name="pos"></param>
        public virtual void Move(Point pos)
        {
            Pos = pos;
            Draw();
        } 

        protected void MoveBase()
        {
            Pos.X += Dir.X;
            Pos.Y += Dir.Y;

            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        }

        /// <summary>
        /// Движение слева на право
        /// </summary>
        /// <param name="delta"></param>
        protected void MoveLeftToRight(int delta = 0)
        {
            Pos.X = Pos.X - Dir.X - (Math.Sign(Dir.X) == -1 ? 1 : -1) * delta;
            if (Pos.X > Game.Width) Pos.X = Size.Width;
            if (Pos.X < 0) Pos.X = Game.Width;
        }

        /// <summary>
        /// Движение справа на лево
        /// </summary>
        /// <param name="delta"></param>
        protected void MoveRightToLeft(int delta = 0)
        {
            Pos.X = Pos.X + Dir.X + (Math.Sign(Dir.X) == -1 ? -1 : 1) * delta;
            if (Pos.X > Game.Width) Pos.X = 0;
            if (Pos.X < 0) Pos.X = Game.Width;
        }

        /// <summary>
        /// Движение снизу в верх
        /// </summary>
        /// <param name="delta"></param>
        protected void MoveTopToBottom(int delta = 0)
        {
            Pos.Y = Pos.Y - Dir.Y - (Math.Sign(Dir.X) == -1 ? 1 : -1) * delta;
            if (Pos.Y > Game.Height) Pos.Y = 0;
            if (Pos.Y < 0) Pos.Y = Game.Height + Size.Height;
        }

        /// <summary>
        /// Движение сверху в низ
        /// </summary>
        /// <param name="delta"></param>
        protected void MoveBottomToTop(int delta = 0)
        {
            Pos.Y = Pos.Y + Dir.Y + (Math.Sign(Dir.X) == -1 ? -1 : 1) * delta;
            if (Pos.Y > Game.Height) Pos.Y = 0;
            if (Pos.Y < 0) Pos.Y = Game.Height + Size.Height;
        }

        /// <summary>
        /// Движение снизу на право
        /// </summary>
        /// <param name="delta"></param>
        protected void MoveBottomToRight()
        {
            if (Pos.Y < 0) Pos.Y = Game.Height;
            Pos.Y = Pos.Y + Game.Random.Next(Game.Height / 4, Pos.Y + Game.Height / 4 + 1) / 32 - Dir.Y;
            if (Pos.Y > Game.Height) Pos.Y = 0;

            if (Pos.X < 0) Pos.X = Game.Width;
            Pos.X = Pos.X - Game.Random.Next(Game.Width / 4, Pos.X + Game.Width / 4 + 1) / 32 + Dir.X;
            if (Pos.X > Game.Width) Pos.X = 0;
        }

        /// <summary>
        /// Движение сверху на лево
        /// </summary>
        /// <param name="delta"></param>
        protected void MoveTopToLeft()
        {
            if (Pos.Y < 0) Pos.Y = Game.Height;
            Pos.Y = Pos.Y - Game.Random.Next(Game.Height / 4, Pos.Y + Game.Height / 4 + 1) / 32 - Dir.Y;
            if (Pos.Y > Game.Height) Pos.Y = 0;

            if (Pos.X < 0) Pos.X = Game.Width;
            Pos.X = Pos.X + Game.Random.Next(Game.Width / 4, Pos.X + Game.Width / 4 + 1) / 32 + Dir.X;
            if (Pos.X > Game.Width) Pos.X = 0;
        }
    }
}
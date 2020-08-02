using Homework1.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1.Data
{
    abstract class BaseObject
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        protected Image Image;

        public BaseObject(Point pos) : this(pos, new Size(5, 5)) { }

        public BaseObject(Point pos, Size size) : this(pos, new Point(0, 0), size) { }

        public BaseObject(Point pos, Point dir, Size size) : this(pos, dir, size, null) { }

        public BaseObject(Point pos, Point dir, Size size, Image image)
        {
            Pos = pos;
            
            /*Если позиция объекта за пределами области, то вернем в область видимости*/
            if (Pos.X < 0) Pos.X += Game.Random.Next(10, Game.Width);
            if (Pos.X > Game.Width) Pos.X = Game.Random.Next(Game.Width / 4, Game.Width / 2  + 1);

            if (Pos.Y < 0) Pos.Y += Game.Random.Next(10, Game.Height);
            if (Pos.Y > Game.Height) Pos.Y = Game.Random.Next(Game.Height / 4, Game.Height / 2 + 1);

            Dir = dir;
            Size = size;
            Image = image;
        }

        public abstract void Draw();

        public abstract void Update();

        protected void MoveBase()
        {
            Pos.X += Dir.X;
            Pos.Y += Dir.Y;

            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        }

        protected void MoveLeftToRight(int delta = 0)
        {
            Pos.X = Pos.X - Dir.X + delta;
            if (Pos.X > Game.Width) Pos.X = 0;
        }

        protected void MoveRightToLeft(int delta = 0)
        {
            Pos.X = Pos.X + Dir.X - delta;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }

        protected void MoveTopToBottom(int delta = 0)
        {
            Pos.Y = Pos.Y - Dir.Y + delta;
            if (Pos.Y > Game.Height) Pos.Y = 0;
        }

        protected void MoveBottomToTop(int delta = 0)
        {
            Pos.Y = Pos.Y + Dir.Y - delta;
            if (Pos.Y < 0) Pos.Y = Game.Height + Size.Height; 
        }
         
        protected void MoveBottomToRight()
        {
            if (Pos.Y < 0) Pos.Y = Game.Height;
            Pos.Y = Pos.Y + Game.Random.Next(Game.Height / 4, Pos.Y + Game.Height / 4 + 1) / 32 - Dir.Y;
            if (Pos.Y > Game.Height) Pos.Y = 0;

            if (Pos.X < 0) Pos.X = Game.Width;
            Pos.X = Pos.X - Game.Random.Next(Game.Width / 4, Pos.X + Game.Width / 4 + 1) / 32 + Dir.X;
            if (Pos.X > Game.Width) Pos.X = 0;
        }

        protected void MoveTopToLeft()
        {
            if (Pos.Y < 0) Pos.Y = Game.Height;
            Pos.Y = Pos.Y - Game.Random.Next(Game.Height/4, Pos.Y + Game.Height / 4 + 1)/32 - Dir.Y;            
            if (Pos.Y > Game.Height) Pos.Y = 0;

            if (Pos.X < 0) Pos.X = Game.Width;
            Pos.X = Pos.X  + Game.Random.Next(Game.Width/4, Pos.X + Game.Width / 4 + 1)/32 + Dir.X;
            if (Pos.X > Game.Width) Pos.X = 0;
        }

        protected void MoveСhaotic()
        {
            int delta = Game.Random.Next(Dir.X > 0 ? -3 : 1, 4);

            Pos.X = Pos.X + Dir.X + delta;
            Pos.Y = Pos.Y + Dir.Y + delta;

            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;

            if (Dir.X <= 0 && Pos.X < 0) Pos.X = Pos.X + Game.Width;
            if (Dir.Y <= 0 && Pos.Y < 0) Pos.Y = Pos.Y + Game.Height;
        }
    }
}

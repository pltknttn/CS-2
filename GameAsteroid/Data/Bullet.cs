using GameAsteroid.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroid.Data
{
    class Bullet : BaseObject
    {
        public int Power = 0;
        public int MoveType = 0;
        
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size) {
            MoveType = Game.Random.Next(0, 4);
            Power = Game.Random.Next(1, 4);
        }

        public override void Draw()
        {
            //Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.FillRectangle(Brushes.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {           
            switch (MoveType)
            {
                case 1:
                    MoveLeftToRight(Power);
                    if (Pos.Y >= Game.Height) Pos.Y = Size.Height;
                    if (Pos.Y <= 0) Pos.Y = Game.Height + Size.Height;
                    break;
                case 2:
                    MoveTopToBottom(Power);
                    if (Pos.X >= Game.Width) Pos.X = Size.Width;
                    break;
                case 3:
                    Pos.X = Pos.X + Power;
                    MoveTopToLeft();
                    break;
                default:
                    MoveRightToLeft(Power);
                    if (Pos.Y >= Game.Height) Pos.Y = Size.Height;
                    if (Pos.Y <= 0) Pos.Y = Game.Height + Size.Height;
                    break;
            } 
        }
    }
}

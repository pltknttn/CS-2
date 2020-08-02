using GameAsteroid.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAsteroid.Data
{
    class Asteroid : BaseObject, ICloneable
    {
        public int MoveType = 0;
        public int Power { get; set; }

        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = Game.Random.Next(1, 8);
            MoveType = Game.Random.Next(0, 3);
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillRectangle(Brushes.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            switch (MoveType)
            {
                case 1:
                    MoveLeftToRight(Power);
                    break;
                case 2:
                    MoveTopToBottom(Power);
                    break;
                default:
                    MoveRightToLeft(Power);
                    break;
            }
        }

        public object Clone()
        {
            return new Asteroid(new Point(Pos.X, Pos.Y), new Point(Dir.X, Dir.Y), new Size(Size.Width, Size.Height))
            {
                Power = this.Power
            };
        }
    }
}

using Homework1.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1.Data
{
    class Rhombus : BaseObject
    {
        public Rhombus(Point pos, Point dir, Size size) : base(pos, dir, size) { }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y + Size.Height/2, Pos.X + Size.Width/2, Pos.Y + Size.Height);
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width/2, Pos.Y + Size.Height, Pos.X + Size.Width, Pos.Y + Size.Height/2);
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y + Size.Height / 2, Pos.X + Size.Width/2, Pos.Y);
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width / 2, Pos.Y, Pos.X + Size.Width, Pos.Y + Size.Height / 2);
        }

        public override void Update()
        {
            MoveBottomToTop();
            MoveLeftToRight();
        }

    }
}

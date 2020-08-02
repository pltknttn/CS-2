using Homework1.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1.Data
{
    class Circle : BaseObject
    {
        public Circle(Point pos, Point dir, Size size) : base(pos, dir, size) { }
         
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawEllipse(Pens.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        } 
        public override void Update()
        {
            MoveBase();
        }
    }
}

using Homework1.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1.Data
{ 
    class Planet : BaseObject
    {
        public Planet(Point pos, Point dir, Size size, Image image) : base(pos, dir, size, image) { }

        public override void Draw()
        {
            if (Image == null)
                Game.Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height)); 
            else
                Game.Buffer.Graphics.DrawImage(Image, Pos.X - Size.Width, Pos.Y - Size.Height);
        }

        public override void Update()
        {
             
        }
    }
}

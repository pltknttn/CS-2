using Homework1.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1.Data
{ 
    class Asteroid : BaseObject
    {
        Bitmap original;

        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size) {
            original = new Bitmap(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream("Homework1.Images.asteroid_30px.png"));
        }

        public override void Draw()
        {
             Game.Buffer.Graphics.DrawImage(original, Pos.X - Size.Width, Pos.Y - Size.Height);
        }

        public override void Update()
        {
            MoveBottomToRight();
        }
    }
}

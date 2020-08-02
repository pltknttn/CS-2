using Homework1.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1.Data
{
    class Rocket : BaseObject
    {
        static Bitmap original = new Bitmap(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream("Homework1.Images.rocket_100px.png")); 
        public Rocket(Point pos, Point dir, Size size) : base(pos, dir, size, original) { 
        }

        public override void Draw()
        { 
            Game.Buffer.Graphics.DrawImage(original, Pos.X - Size.Width, Pos.Y - Size.Height);
        }

        public override void Update()
        {
            MoveTopToLeft();
        }
    }
}

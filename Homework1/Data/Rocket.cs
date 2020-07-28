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
        public Rocket(Point pos, Point dir, Size size) : base(pos, dir, size) { }

        public override void Draw()
        {
            Bitmap original = new Bitmap(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream("Homework1.Images.rocket_100px.png"));
            Game.Buffer.Graphics.DrawImage(original, Pos.X - Size.Width, Pos.Y - Size.Height);
        }

        public override void Update()
        {
            MoveTopToLeft();
        }
    }
}

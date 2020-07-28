using Homework1.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1.Data
{
    class Spacecraft: BaseObject
    {
        public Spacecraft(Point pos, Point dir, Size size) : base(pos, dir, size) { }

        public override void Draw()
        {
            Bitmap original = new Bitmap(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream("Homework1.Images.icons8_space_shuttle_32.png")); 
            Game.Buffer.Graphics.DrawImage(original, Pos.X - Size.Width, Pos.Y - Size.Height); 
        }

        public override void Update()
        {
            MoveBottomToTop();
            MoveLeftToRight();
        }
    }
}

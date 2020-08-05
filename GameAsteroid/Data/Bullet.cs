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
        public int Step = 0;

        public Bullet(Point pos, Point dir, Size size): this(pos, dir, size, 0, 0)
        { } 
        public Bullet(Point pos, Point dir, Size size, int move, int step) : base(pos, dir, size) {
            MoveType = move;
            Step = step;
            Power = Game.Random.Next(4, 21);
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillRectangle(Brushes.OrangeRed, Pos.X+Step, Pos.Y+Step, Size.Width, Size.Height); 
        }

        public override void Update()
        {
            switch (MoveType)
            {
                case 0:                                       
                    Pos.X += Dir.X + Power; 
                    break;
                default:
                    Pos.Y -= Dir.Y + Power/2; 
                    Pos.X += Dir.X + Power/2; 
                    break;
            } 
        }

        public override string GetName()
        {
            return $"Bullet_{ObjectUid}";
        }
    }
}

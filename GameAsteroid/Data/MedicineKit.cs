using GameAsteroid.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroid.Data
{
    class MedicineKit : BaseObject
    {
        private int _energy = 100;

        public MedicineKit(Point pos, Point dir, Size size, int energy) : base(pos, dir, size) {
           
            if (energy <= 0 || energy > Game.MAX_ENERGY) throw new GameObjectException($"Значение энергии больше 0 и меньше {Game.MAX_ENERGY}.");

            _energy = energy;
        }

        public int Energy => _energy;

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(GameResources.MedKitImage, Pos.X, Pos.Y, Size.Width, Size.Height); 
        }

        public override void Update()
        { 
        }

        public override string GetName()
        {
            return $"MedicineKit_{ObjectUid}";
        }
    }
}

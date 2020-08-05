using GameAsteroid.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroid.Data
{
    class Ship : BaseObject, IMove
    {        
        private int _energy = 100;
        private int _hitCounter = 0;
        private int _shootsCounter = 0;
       
        public int Energy => _energy;

        public int HitCounter => _hitCounter;

        public int ShootsCounter => _shootsCounter;

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size) { }

        public void EnergyLow(int n)
        {
            if (_energy > n)
                _energy -= n;
            else
                _energy = 0;

            Game.ActionWriteLog?.Invoke($"{GetName()}: EnergyLow {_energy}");
        }

        public void EnergyHigh(int n)
        {
            if (_energy + n < Game.MAX_ENERGY)
                _energy += n;
            else
                _energy = Game.MAX_ENERGY;

            Game.ActionWriteLog?.Invoke($"{GetName()}: EnergyHigh {_energy}");
        }

        public void Strike(int n)
        {
            _shootsCounter += n;
            Game.ActionWriteLog?.Invoke($"{GetName()}: Strike {_shootsCounter}");
        }

        public void Slay(int n)
        {
            _hitCounter += n;
            Game.ActionWriteLog?.Invoke($"{GetName()}: Slay {_hitCounter}");
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(GameResources.ShipImage, Pos.X, Pos.Y, Size.Width, Size.Height); 
        }

        public override void Update()
        { 
        }

        #region IMove
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y -= Dir.Y;
        }
                
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y += Dir.Y;
        }

        public void Left()
        {
            if (Pos.X > 0) Pos.X -= Dir.Y;
        }

        public void Right()
        {
            if (Pos.X < Game.Width) Pos.X += Dir.X;
        }

        public void MouseMove()
        {
            
        }
        #endregion

        public static event Message MessageDie; 

        public void Die()
        {
            MessageDie?.Invoke();
            Game.ActionWriteLog?.Invoke($"{GetName()}: Die");
        }

        public override string GetName()
        {
            return $"Ship_{ObjectUid}";
        }
    }
}

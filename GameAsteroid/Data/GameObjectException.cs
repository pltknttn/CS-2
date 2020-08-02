using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroid.Data
{
    class GameObjectException: Exception
    {
        public GameObjectException(string message) : base(message) { }
    }
}

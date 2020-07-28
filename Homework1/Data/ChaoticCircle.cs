using Homework1.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1.Data
{
    class ChaoticCircle : BaseObject
    {
        public ChaoticCircle(Point pos, Point dir, Size size) : base(pos, dir, size) { }
         
        public override void Update()
        {
            MoveСhaotic();
        }
    }
}

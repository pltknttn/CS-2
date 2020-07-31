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
         
        public override void Update()
        {
             
        }
    }
}

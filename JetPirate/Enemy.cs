using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JetPirate
{
    internal class Enemy : Object2D
    {
        public Enemy(Vector2 pos, float rot) : base (pos, rot) { }
        public float Heealth;
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JetPirate
{
    abstract class Object2D 
    {
        protected Vector2 position;

        protected float rotation;
        protected float Rotation
        {
            get => rotation;
            set
            {
                rotation = ModulasClamp(value, (float)-Math.PI * 2, (float)Math.PI * 2);
            }
        }

        public Object2D(Vector2 pos, float rot)
        {
            this.position = pos;
            this.rotation = rot;
        }




        /// <summary>
        /// Returns min+Value%Range and max-Value%Range, where Range is Max-Min. Helpful for rotation when value should repeat from other side of Range if value is out of Range.        
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static public float ModulasClamp(float value, float min, float max)
        {
            float ret;
            if (value >= max)
            {
                ret = min + value % (max - min);
                return ret;
            }
            else if (value < min)
            {
                ret = (max) - Math.Abs(value % (max - min));
                return ret;
            }

            return value;

        }


    }
}

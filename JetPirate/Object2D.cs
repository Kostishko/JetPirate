using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JetPirate
{
    abstract public class Object2D 
    {
        protected Vector2 position;

        private float rotation;

//        protected Rectangle physicRec;

        public bool isPhysicActive;
        
        protected float Rotation
        {
            get => rotation;
            set
            {
                rotation = ModulasClamp(value, (float)-Math.PI, (float)Math.PI);
            }
        }

        public Object2D(Vector2 pos, float rot)
        {
            this.position = pos;
            this.Rotation = rot;
         //   this.physicRec = new Rectangle(); //default rectangle

            isPhysicActive = false;
        }

        /// <summary>
        /// Get an Object2d that can be checked if it an Enemy, buff or smth else
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Collided(Object2D obj)
        {
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
                ret = min + value % (max);
                return ret;
            }
            else if (value < min)
            {
                ret = (max) - Math.Abs(value % (max));
                return ret;
            }

            return value;

        }

        public float GetRotation()
        {
            return Rotation;
        }

        public Vector2 GetPosition()
        {
            return position;
        }


        /// <summary>
        /// Return parent for physic module and this for Object2D
        /// </summary>
        /// <returns></returns>
        public virtual Object2D GetParent()
        {
            return this;
        }

        //public Rectangle GetRectangle()
        //{   
        //      return physicRec;
        //}

    }
}

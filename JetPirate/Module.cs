using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JetPirate
{
    /// <summary>
    /// Object that follow and rotate after the parent object even if it is not at the centre of parent object
    /// </summary>
    abstract class Module : Object2D
    {
        protected Object2D parent;
        protected Vector2 shiftPosition;
        protected float shiftRotation;
        protected float distance;

        public Module(Object2D par, Vector2 shift) : base(par.GetPosition(), par.GetRotation())
        {
            parent = par;
            shiftPosition = shift;
            shiftRotation = (float)Math.Atan2(shift.Y, shift.X);
            distance = (float)Math.Sqrt(Math.Pow(shift.X, 2)+ Math.Pow(shift.Y, 2));
        }

        public void UpdateMe()
        {
            Rotation = parent.GetRotation()+shiftRotation;
            shiftPosition = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation) )*Math.Clamp(distance, 1, float.MaxValue);
            position = parent.GetPosition()+shiftPosition;

            //physicRec.Location = (position-shiftPosition).ToPoint();
        }

        public void DrawMe(SpriteBatch sp)
        {

        }

        /// <summary>
        /// Return parent
        /// </summary>
        /// <returns></returns>
        public override Object2D GetParent()
        {
            return parent;
        }

    }
}

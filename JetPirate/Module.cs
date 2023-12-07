using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JetPirate
{
    abstract class Module : Object2D
    {
        protected Object2D parent;
        protected Vector2 shiftPosition;
        protected float shiftRotation;
        protected float distance;

        public Module(Vector2 pos, float rot, Object2D par, Vector2 shift) : base(pos, rot)
        {
            parent = par;
            shiftPosition = shift;
            shiftRotation = (float)Math.Atan2(shift.Y, shift.X);
            distance = (float)Math.Sqrt(Math.Pow(shift.X, 2)+ Math.Pow(shift.Y, 2));
        }


        public void UpdateMe()
        {
            Rotation = parent.GetRotation()+shiftRotation;
            shiftPosition = new Vector2((float)Math.Cos(Rotation) * distance, (float)Math.Sin(Rotation) * distance);
            position = parent.GetPosition()+shiftPosition;
        }

        public void DrawMe(SpriteBatch sp)
        {

        }

    }
}

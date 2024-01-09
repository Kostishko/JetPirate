using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JetPirate
{
    internal class PhysicModule : Module
    {

        public Rectangle physicRec;
        /// <summary>
        /// Module with body
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="parent"></param>
        /// <param name="shift"></param>
        /// <param name="rectangleSize">Size of physic rectangle</param>
        public PhysicModule(Object2D parent, Vector2 shift, Vector2 rectangleSize) : base(parent, shift)
        {
            //physic
            physicRec = new Rectangle((int)Math.Round(parent.GetPosition().X), (int)Math.Round(parent.GetPosition().Y), (int)Math.Round(rectangleSize.X), (int)Math.Round(rectangleSize.Y));
            PhysicManager.AddObject(this);
        }

        public new void UpdateMe()
        {
            base.UpdateMe();
            physicRec.Location = (position- new Vector2(physicRec.Width/2, physicRec.Height/2)).ToPoint();
            ///treatment of parent rotation.

        }

        public new void Collided(Object2D obj)
        {
            parent.Collided(obj);
        }

        public Rectangle GetRectangle()
        {
            return physicRec;
        }


    }
}

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
    /// <summary>
    /// this is a module that can be attached to anything and it will register any collisions with any other same module
    /// and sent the data about collided object to their parent
    /// </summary>
    internal class PhysicModule : Module
    {



        private Rectangle physicRec;
        private Vector2 rectangleHalfSize;

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
            rectangleHalfSize = new Vector2(physicRec.Width / 2, physicRec.Height / 2);
            isPhysicActive = true;
            PhysicManager.AddObject(this);
        }

        public new void UpdateMe()
        {
            base.UpdateMe();
            // following for rectangle (it can't rotate)
            physicRec.Location = (position- rectangleHalfSize).ToPoint();
            ///treatment of parent rotation.

        }

        public new void Collided(Object2D obj) // sending collide and information about collided object to parent
        {
         
            
                parent.Collided(obj.GetParent());
            
        }

        /// <summary>
        /// Return the physic rectangle
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRectangle()
        {
            return physicRec;
        }


    }
}

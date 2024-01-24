using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JetPirate
{
    /// <summary>
    /// Water is a bottom bound that damage the jet and teleport it to safe zone
    /// </summary>
    internal class Water : Object2D
    {

        //spriteSheet
        private Texture2D texture;
        private Vector2 rightShift;

        //frame changing
        private Rectangle frame;
        private float frameTimer;

        //player
        private JetShip jetShip;


        //Collision element
        private PhysicModule physicModule;


        public Water(Vector2 pos, float rot, JetShip jet, ContentManager content) : base (pos, rot) 
        {
            //sprite with two layers to change them as frames
            texture = content.Load<Texture2D>("Sprites/WaterSpriteSheet");

            frame = new Rectangle(0,0,texture.Width,texture.Height/2);
            rightShift = new Vector2(texture.Width, 0);

            jetShip = jet;

            physicModule = new PhysicModule(this, new Vector2(0,80), new Vector2(texture.Width*3, texture.Height/2));

        }  

        public void UpdateMe()
        {


            physicModule.UpdateMe();

            //Shift the jet back to safe from water space
            if (jetShip.GetPosition().X >= (position + rightShift).X)
            {
                position += rightShift;
            }

            if (jetShip.GetPosition().X <= (position - rightShift / 2).X)
            {
                position -= rightShift;
            }

            //frame change timer 
            if (frameTimer <= 0) 
            {
                NextFrame();
                frameTimer = 1f;
            }
            else
            {
                frameTimer -= 0.1f;
            }

        }


        public void DrawMe(SpriteBatch sp)
        {   sp.Draw(texture, position - rightShift, frame, Color.White);
            sp.Draw(texture, position, frame, Color.White);
            sp.Draw(texture, position + rightShift, frame,  Color.White);

            //debug
           // DebugManager.DebugRectangle(physicModule.GetRectangle());

        }

        /// <summary>
        /// Change the frame
        /// </summary>
        private void NextFrame()
        {
            if(frame.Location.Y+ texture.Height/2>= texture.Height)
            {
                frame.Location = new Vector2(0, 0).ToPoint();
            }
            else
            {
                frame.Location += new Vector2(0, +texture.Height/2).ToPoint();
            }
        }

        /// <summary>
        /// Collision with the jet
        /// </summary>
        /// <param name="obj"></param>
        public override void Collided(Object2D obj)
        {
            if (obj is JetShip)
            {
                JetShip jet = (JetShip)obj;
                //damage
                jet.TakeDamage();
                //back the jet in air from the water
                jet.SetPosition(new Vector2(jetShip.GetPosition().X, jetShip.GetPosition().Y - 1000));
            }


        }

    }
}

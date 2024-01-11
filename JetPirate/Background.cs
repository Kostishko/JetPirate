using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JetPirate
{
    internal class Background : Object2D
    {

        private ContentManager content;
        private Texture2D texture;
        private JetShip jetShip;

        

        //shifts for left and right pictures
        private Vector2 rightShift;

        public Background(Vector2 pos, float rot, ContentManager content, JetShip ship ) : base (pos, rot)
        {
            this.content=content;
            texture = this.content.Load<Texture2D>("Sprites/Background");

            jetShip = ship;

            position = new Vector2(-texture.Width/2, -texture.Height);
            rightShift = new Vector2(texture.Width,0);



        }

        public void UpdateMe()
        {
            if(jetShip.GetPosition().X>=(position + rightShift).X)
            {
                position += rightShift;
            }

            if(jetShip.GetPosition().X<=(position -rightShift/2).X)
            {
                position -= rightShift;
            }
        }



        public void DrawMe(SpriteBatch sp)
        {
            sp.Draw(texture, position - rightShift, Color.White);
            sp.Draw(texture, position, Color.White);
            sp.Draw(texture, position + rightShift, Color.White);

        }


    }
}

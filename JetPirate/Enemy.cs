using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace JetPirate
{
    internal class Enemy : Object2D
    {

        //physic
        

        //characteristics
        public float Health;


        //visual
        private Texture2D texture;        

        public Enemy(Vector2 pos, float rot, Texture2D tex) : base (pos, rot) 
        {
            texture = tex;
           // physicRec = new Rectangle((int)Math.Round(pos.X),(int)Math.Round( pos.Y), tex.Width,tex.Height );
            Health = 100f;
            isPhysicActive = true;
           // PhysicManager.AddObject(this);  
        }
       


        public void TakeDamage(float damage)
        {
            Health-=damage;
            if(Health <= 0)
            {
                this.Destroyed();
            }
        }

        public void Destroyed()
        {
            isPhysicActive=false;
          //  PhysicManager.RemoveObject(this);
        }

        public void DrawMe(SpriteBatch sp)
        {
            if (isPhysicActive)
            {
                sp.Draw(texture, position, null, Color.White, Rotation, Vector2.Zero, 1f, SpriteEffects.None, 1);
               // DebugManager.DebugRectangle(physicRec);
            }
        }


    }
}

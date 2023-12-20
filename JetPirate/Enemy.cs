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

         physicRec = new Rectangle((int)Math.Round(pos.X),(int)Math.Round( pos.Y), tex.Width,tex.Height );
        PhysicManager.AddObject(this);
            
        }
       


        public void GetDamage(float damage)
        {
            Health-=damage;
            if(Health > 0)
            {
                this.Destroyed();
            }
        }

        public void Destroyed()
        {
            PhysicManager.RemoveObject(this);
        }
    }
}

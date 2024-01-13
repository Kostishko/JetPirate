using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace JetPirate
{
    internal class Enemy : Object2D
    {

        //Enemy management
        private bool isActive;
        private EnemyManager EnemyManager;

        //physic
        private PhysicModule physicModule;

        //characteristics        
        public float speed;

        //visual
        private Texture2D texture;        

        

        public Enemy(Vector2 pos, float rot, EnemyManager enemyManager) : base (pos, rot) 
        {
            EnemyManager = enemyManager;
            isActive = false;

            physicModule = new PhysicModule(this, Vector2.Zero, new Vector2(texture.Width/2,texture.Height));


        }
       

        public void UpdateMe()
        {
            //physic update
            physicModule.UpdateMe();
        }

        public void DrawMe(SpriteBatch sp)
        {
            sp.Draw(texture,)

        }


        public void TakeDamage(float damage)
        {
                this.Destroyed();       
        }

        public void ResetMe(float speed, )
        {

        }

        public void Destroyed()
        {
            isPhysicActive=false;
          
        }


    }
}

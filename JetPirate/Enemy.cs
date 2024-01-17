using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace JetPirate
{
    internal class Enemy : Object2D
    {

        //Enemy management
        private bool isActive;
        private EnemyManager EnemyManager;
        private ContentManager content;

        //physic
        private PhysicModule physicModuleOne;
        private PhysicModule physicModuleTwo;

        //characteristics        
        public float speed;
        private Vector2 velocity;
        private Vector2 pivotPoint;

        //visual
        private Texture2D texture;        
        private ParticleSystem engineParticles; // to think - maybe I need a module for them? 
        private ParticleSystem explosionParticles;
        private float explosionTime; //time and timer for particles of explosion
        private float explosionTimer;





        public Enemy(Vector2 pos, float rot, EnemyManager enemyManager, ContentManager content) : base (pos, rot) 
        {
            EnemyManager = enemyManager;
            isActive = false;
            this.content = content;

            //Not too smart desicion, could be a list but I have to do as fast as possible at this moment
            physicModuleOne = new PhysicModule(this, new Vector2(65,0), new Vector2(65,40));
            physicModuleOne.isPhysicActive = false;
            physicModuleTwo = new PhysicModule(this, new Vector2(-65, 0), new Vector2(65, 40));
            physicModuleTwo.isPhysicActive = false;

            engineParticles = new ParticleSystem(position, Rotation, content.Load<Texture2D>("fire_left"), 2f, 15f, 0.2f, 0.5f);
            explosionParticles = new ParticleSystem(position, Rotation, content.Load<Texture2D>("Particles/ExploudParticle"), 1f, 25f, 0f, (float)Math.PI);




            //Visual
            explosionTime = 3; //time of explosion


        }
       

        public void UpdateMe()
        {
            if (isActive)
            {               
                position += velocity * speed;
                CheckBorders();
               

                //Particles movement to this enemy
                engineParticles.UpdateMe(position, Rotation);
                explosionParticles.UpdateMe(position, Rotation);                

                //physic update
                physicModuleOne.UpdateMe();               
                physicModuleTwo.UpdateMe();

                //

            }
        }

        public void DrawMe(SpriteBatch sp)
        {
            if (isActive) //drawning only if the enemy is active
            {
                sp.Draw(texture, position, null, Color.White, Rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                engineParticles.DrawMe(sp);               
                //add engine particles here
            }
            if (!isActive)
            {
                if (explosionTimer > 0)
                {
                    explosionParticles.Play();
                    explosionParticles.DrawMe(sp);
                    explosionTimer -= 0.1f;

                }
                else
                {
                    explosionParticles.Stop();
                }
            }
        }

        // Old one, to delete
        //public void TakeDamage(float damage)
        //{
        //        this.Destroyed();       
        //}

        public void ResetMe(float speed, Vector2 shipPos, Vector2 startPos , Texture2D texture)
        {            
            this.speed = speed;
            pivotPoint = shipPos;
            position = startPos;
            velocity = new Vector2(position.X - pivotPoint.X, position.Y - pivotPoint.Y);
            velocity.Normalize();
            this.texture = texture;
            texture = EnemyManager.GetTexture();
            isActive = true;
            physicModuleOne.isPhysicActive = true;
            physicModuleTwo.isPhysicActive=true;
        }

        public void CheckBorders()
        {
            if(position.X<EnemyManager.GetLeftUpBorder().X||position.Y< EnemyManager.GetLeftUpBorder().Y)
            {
                Destroyed();
            }

            if(position.X>EnemyManager.GetRightDownBorder().X||position.Y>EnemyManager.GetRightDownBorder().Y)
            {
                Destroyed();
            }

        }

        public void Destroyed()
        {

            isActive = false;
            physicModuleOne.isPhysicActive = false;
            physicModuleTwo.isPhysicActive = false;
            EnemyManager.enemyCounter++;

            //explosion visual
            explosionTimer = explosionTime;
        }

        public override void Collided(Object2D obj)
        {
            if(obj is JetShip)
            {
                JetShip jet = (JetShip)obj;
                jet.TakeDamage();
                Destroyed();                
            }
        }

        public bool GetEnemyState()
        {
            return isActive;
        }


    }
}

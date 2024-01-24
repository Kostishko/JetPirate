using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace JetPirate
{
    /// <summary>
    /// Enemy that spawned around the ship
    /// </summary>
    public class Enemy : Object2D
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
            physicModuleOne = new PhysicModule(this, new Vector2(25,15), new Vector2(20,20));
            physicModuleOne.isPhysicActive = false;
            physicModuleTwo = new PhysicModule(this, new Vector2(50, 15), new Vector2(20, 20));
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
                CheckBorders(); // destroyinfg if too far from the player ship
               

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
                sp.Draw(texture, position, null, Color.White, Rotation, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                engineParticles.DrawMe(sp);
                //DebugManager.DebugRectangle(physicModuleOne.GetRectangle());
                //DebugManager.DebugRectangle(physicModuleTwo.GetRectangle());
                //add engine particles here
            }
            if (!isActive) // should draw particles of explosion after death - but there are some bug - works not every time
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


        /// <summary>
        /// Reset after death, or in the end of game
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="shipPos"></param>
        /// <param name="startPos"></param>
        /// <param name="texture"></param>
        public void ResetMe(float speed, Vector2 shipPos, Vector2 startPos , Texture2D texture)
        {            
            this.speed = speed;
            pivotPoint = shipPos;
            position = startPos;
            velocity = new Vector2(pivotPoint.X- position.X , pivotPoint.Y-position.Y );
            Rotation = (float)Math.Atan2(velocity.Y, velocity.X)- (float)Math.PI;
            velocity.Normalize();
            this.texture = texture;
            texture = EnemyManager.GetTexture();
            isActive = true;
            physicModuleOne.isPhysicActive = true;
            physicModuleTwo.isPhysicActive=true;
        }

        /// <summary>
        /// Destroying the enemy if it is too far from the player jet
        /// </summary>
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

        /// <summary>
        /// Back this enemy in the pull after deth
        /// </summary>
        public void Destroyed()
        {

            isActive = false;
            physicModuleOne.isPhysicActive = false;
            physicModuleTwo.isPhysicActive = false;
            EnemyManager.enemyCounter++;

            //explosion visual
            explosionTimer = explosionTime;
        }

        /// <summary>
        /// Deliver the damage to the player jet
        /// </summary>
        /// <param name="obj"></param>
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

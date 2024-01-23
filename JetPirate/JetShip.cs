using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Threading.Tasks.Sources;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace JetPirate
{
    /// <summary>
    /// Jet ship is a player controlled object - main character of the game
    /// </summary>
    public class JetShip : Object2D
    {

        //Visual
        private Texture2D texture;
        private Vector2 origin;

        private EngineParticles leftEngine;
        private EngineParticles rightEngine;

        //Weapon
        private ShipGun shipGun;



        //Movement variables (there are variable of real velocity and plan velocity to create the effect of inertion)
        #region Movement variables
        //Rotation heritage from Object2D
        private float rightRotation;
        private float leftRotation;


        private float maxPower; //Max power for both engines
        //engines' powers. Clamp as half of maxPower. Sum them to final power;
        private float leftPower;
        private float LeftPower
        {
            get
            {
                return leftPower;
            }
            set
            {
                leftPower = Math.Clamp(value, 0, maxPower / 2);
            }
        } //left power, max clamp with maxPower/2 as max
        private float rightPower;
        private float RightPower
        {
            get
            {
                return rightPower;
            }
            set
            {
                rightPower = Math.Clamp(value, 0, maxPower / 2);
            }
        }//right power
        public float currentPower
        {
            get => Math.Clamp(LeftPower + RightPower, 0, maxPower);
        }  //variable for sum of leftPower and RightPower, only get
        private float maxGravity; // use gravity here because there is no more objects that should be affected by that

        private float currentGravity;
        private float CurrentGravity
        {
            get
            {
                return currentGravity;
            }
            set
            {
                currentGravity = Math.Clamp(value, 0, maxGravity);
            }
        } //Gravity will less if power closer to max

        private Vector2 velocity; // real velocity of jet
        private Vector2 planVelocity; // velocity that controlled by player 
        private float velX, velY; //variables for inertion of final real velocity
        #endregion

        //Physic rectangles for collisions
        private List<PhysicModule> physicsModules;

        //Camera (access to effects)
        private Camera camera;

        #region Damage and health

        //Technique variables
        private int health;
        private int maxHealth;
        private float invulTimer;
        private float invulTime;
        private bool isAlive;
        private Vector2 startPos;

        //Visual
        private ParticleSystem explosionsParticles;
        private ParticleSystem piecesParticles;
        private float explosionTime;
        private float explosionTimer;

        #endregion


        public JetShip(Vector2 pos, float rot, ContentManager content, Camera cam) : base(pos, rot)
        {
            texture = content.Load<Texture2D>("Sprites/Ship_03");
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            maxGravity = 2f;
            maxPower = 6f;

            camera = cam;




            //visual effects of engines
            leftEngine = new EngineParticles(this, new Vector2(-45, 35), content.Load<Texture2D>("fire_left"));
            rightEngine = new EngineParticles(this, new Vector2(45, 35), content.Load<Texture2D>("fire_left"));

            //visual effects of damage
            explosionsParticles = new ParticleSystem(position, Rotation, content.Load<Texture2D>("Particles/ExploudParticle"), 1f, 25f, 0f, (float)Math.PI);
            piecesParticles = new ParticleSystem(position, Rotation, content.Load<Texture2D>("Particles/ParticleBit"), 4f, 10f, 0f, (float)Math.PI);
            explosionTime = 1f;
            explosionTimer = 0f;

            //Weapon
            shipGun = new ShipGun(this, new Vector2(texture.Width / 9 - 14, texture.Height / 5), content);

            //Physic rectangles for collisions
            physicsModules = new List<PhysicModule>();
            physicsModules.Add(new PhysicModule(this, new Vector2(-45, -5), new Vector2(texture.Width / 5, texture.Width / 5)));
            physicsModules.Add(new PhysicModule(this, new Vector2(45, -5), new Vector2(texture.Width / 5, texture.Width / 5)));
            physicsModules.Add(new PhysicModule(this, new Vector2(45, 25), new Vector2(texture.Width / 5, texture.Width / 5)));
            physicsModules.Add(new PhysicModule(this, new Vector2(-45, 25), new Vector2(texture.Width / 5, texture.Width / 5)));
            physicsModules.Add(new PhysicModule(this, new Vector2(0, 25), new Vector2(texture.Width / 4, texture.Width / 4)));
            physicsModules.Add(new PhysicModule(this, new Vector2(0, -15), new Vector2(texture.Width / 4, texture.Width / 4)));

            #region Restoring and health
            maxHealth = 5;
            invulTime = 30f;
            startPos = pos;

            Restore();
            #endregion


        }


        public void UpdateMe(GamePadState gPad, GamePadState oldGPad, GameTime gTime)
        {
            //Weapon
            shipGun.UpdateMe(gPad, oldGPad);

            #region movement
            //Rotate and increse power with Triggers and decrease that if Triggers released
            if (gPad.Triggers.Right != 0)
            {
                if (gPad.Triggers.Left == 0)
                {
                    leftRotation -= 0.02f * gPad.Triggers.Right;
                }
                //Rotation -= 0.02f*gPad.Triggers.Left;
                LeftPower += 0.01f * gPad.Triggers.Right;
                rightEngine.UpdateMe(true);
            }
            else
            {
                LeftPower -= 0.05f;
                rightEngine.UpdateMe(false);
            }

            if (gPad.Triggers.Left != 0)
            {
                if (gPad.Triggers.Right == 0)
                {
                    rightRotation += 0.02f * gPad.Triggers.Left;
                }
                //Rotation += 0.02f * gPad.Triggers.Right;
                RightPower += 0.01f * gPad.Triggers.Left;
                leftEngine.UpdateMe(true);
            }
            else
            {
                RightPower -= 0.05f;
                leftEngine.UpdateMe(false);
            }

            //Inertion for rotation
            if (leftRotation < 0)
            {
                Rotation -= 0.015f;
                leftRotation += 0.015f;
            }
            if (rightRotation > 0)
            {
                Rotation += 0.015f;
                rightRotation -= 0.015f;
            }


            //Gravitation compute (maybe add something more complex later)
            CurrentGravity = maxGravity;

            // velocity that create player
            planVelocity = new Vector2((float)Math.Sin(Rotation) * currentPower, -(float)Math.Cos(Rotation) * currentPower + CurrentGravity);

            //Inertion for velocity
            if (velX != planVelocity.X)
            {
                if (velX > planVelocity.X)
                {
                    velX = Math.Clamp(velX - 0.02f, planVelocity.X - 0.02f, 10);
                }
                else
                {
                    velX = Math.Clamp(velX + 0.02f, -100, planVelocity.X + 0.02f);
                }
            }
            if (velY != planVelocity.Y)
            {
                if (velY > planVelocity.Y)
                {
                    velY = Math.Clamp(velY - 0.02f, planVelocity.Y - 0.02f, 10);
                }
                else
                {
                    velY = Math.Clamp(velY + 0.02f, -100, planVelocity.Y + 0.02f);
                }
            }

            velocity = new Vector2(velX, velY);
            position += velocity;

            #endregion

            #region physic
            for (int i = 0; i < physicsModules.Count; i++)
            {
                physicsModules[i].UpdateMe();
            }


            #endregion


            #region health and damage
            //visual effects
            explosionsParticles.UpdateMe(position, Rotation);
            piecesParticles.UpdateMe(position, Rotation);
            if (explosionTimer > 0)
            {
                explosionTimer -= 0.1f;
                explosionsParticles.Play();
                piecesParticles.Play();
            }
            else
            {
                explosionsParticles.Stop();
                piecesParticles.Stop();
            }

            //test
            if (gPad.Buttons.Y == ButtonState.Released && oldGPad.Buttons.Y == ButtonState.Pressed)
            {
                TakeDamage();
            }

            if (invulTimer > 0)
            {
                invulTimer -= 0.1f;
            }




            #endregion
        }


        public void DrawMe(SpriteBatch sp)
        {

            //drawing the ship if it's alive
            if (isAlive)
            {
                //engines particles
                leftEngine.engineParticles.DrawMe(sp);
                rightEngine.engineParticles.DrawMe(sp);

                if (invulTimer > 0)
                {
                    if ((int)Math.Round(invulTimer) % 2 == 0)
                    {

                    }
                    else
                    {
                        sp.Draw(texture, position, null, Color.White, Rotation, origin, 1f, SpriteEffects.None, 1f);
                    }
                }
                else
                {
                    sp.Draw(texture, position, null, Color.White, Rotation, origin, 1f, SpriteEffects.None, 1f);
                }
                //jet itself


                //gun
                shipGun.GetGun().DrawMe(sp);
            }


            //damage visual
            piecesParticles.DrawMe(sp);
            explosionsParticles.DrawMe(sp);

            #region debug
            //DebugManager.DebugString("current power: " + currentPower, new Vector2(0, 0));
            //DebugManager.DebugString("Rotation: "+ Rotation, new Vector2(0, 22));    
            //DebugManager.DebugString("Planotation: "+PlanRotation, new Vector2(0, 44));
            //DebugManager.DebugString("Origin: " + origin, new Vector2(0, 66));

            // DebugManager.DebugString("LeftEngine distance: " + leftEngine.distance, new Vector2(0,0));

            for (int i = 0; i < physicsModules.Count; i++)
            {
                DebugManager.DebugRectangle(physicsModules[i].GetRectangle());
            }
            #endregion
        }


        //collision treatment 
        public override void Collided(Object2D obj)
        {
            //Meet Enemy - the deleting one health point
            if (obj is Enemy)
            {
                if (invulTimer <= 0)
                {
                    //enemy destroying
                    Enemy enemy = (Enemy)obj;
                    enemy.Destroyed();

                    //ship damage
                    TakeDamage();
                }

            }
        }


        #region health and damage getting
        /// <summary>
        /// Current health of the ship
        /// </summary>
        /// <returns></returns>
        public int GetHealth()
        {
            return health;
        }

        /// <summary>
        /// The Ship always get 1 damage and go to invulable state for short time
        /// </summary>
        public void TakeDamage()
        {
            if (invulTimer <= 0)
            {
                health--;
                camera.StartShaking(10);
                //explosions particles
                explosionTimer = explosionTime;
                invulTimer = invulTime;
                //death treatment
                if (health == 0)
                {
                    isAlive = false;
                }
            }
        }

        /// <summary>
        /// Set position if it's needed to teleport the jet
        /// </summary>
        /// <param name="pos"></param>
        public void SetPosition(Vector2 pos)
        {
            position = pos;
        }

        /// <summary>
        /// Restore the ship when we need
        /// </summary>
        public void Restore()
        {
            health = maxHealth;
            invulTimer = 0;
            position = Vector2.Zero;
            isAlive = true;
            position = startPos;
        }


        #endregion

        /// <summary>
        /// Return the current state of gun
        /// </summary>
        /// <returns></returns>
        public Gun GetGun()
        {
            return shipGun.GetGun();
        }
    }
}

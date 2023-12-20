using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks.Sources;
using static System.Net.Mime.MediaTypeNames;


namespace JetPirate
{
    internal class JetShip : Object2D
    {

        //Visual
        private Texture2D texture;
        private Vector2 origin;

        private EngineParticles leftEngine;
        private EngineParticles rightEngine;

        //Weapon
        private ShipGun shipGun;



        //Movement variables (there are variable of real velocity and plan velocity to create the effect of inertion)
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
            get => Math.Clamp(LeftPower+RightPower,0,maxPower);           
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
                currentGravity = Math.Clamp(value, 0,maxGravity);
            }
        } //Gravity will less if power closer to max

        private Vector2 velocity; // real velocity of jet
        private Vector2 planVelocity; // velocity that controlled by player 
        private float velX, velY; //variables for inertion of final real velocity


        public JetShip(Vector2 pos, float rot, Texture2D tex, Texture2D engineFire, Texture2D shipGunTex) : base(pos,rot)
        {
            texture = tex;
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
            maxGravity = 2f;
            maxPower = 6f;

            //visual effects
            leftEngine = new EngineParticles(pos, rot, this, new Vector2(-25, tex.Height*0.05f), engineFire);
            rightEngine = new EngineParticles(pos, rot, this, new Vector2(tex.Width*0.05f-10, tex.Height * 0.05f), engineFire);

            //Weapon
            shipGun = new ShipGun(pos, rot, this, new Vector2(tex.Width*0.05f/2-15, tex.Height*0.05f/2-15), shipGunTex);

        }


        public void UpdateMe(GamePadState gPad, GamePadState oldGPad, GameTime gTime)
        {
            //Weapon
            shipGun.UpdateMe(gPad, oldGPad);

            #region movement
            //Rotate and increse power with Triggers and decrease that if Triggers released
            if (gPad.Triggers.Right!=0)
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

            if(gPad.Triggers.Left!=0) 
            {
                if (gPad.Triggers.Right == 0)
                {
                    rightRotation += 0.02f * gPad.Triggers.Left;
                }
                //Rotation += 0.02f * gPad.Triggers.Right;
                RightPower+=0.01f* gPad.Triggers.Left;
                leftEngine.UpdateMe(true);
            }
            else
            {
                RightPower -= 0.05f;
                leftEngine.UpdateMe(false);
            }

            //Inertion for rotation
            if (leftRotation<0)
            {
                Rotation -= 0.015f;
                leftRotation += 0.015f;
            }
            if(rightRotation>0)
            {
                Rotation += 0.015f;
                rightRotation -= 0.015f;
            }
            

            //Gravitation compute (maybe add something more complex later)
            CurrentGravity = maxGravity;

            // velocity that create player
            planVelocity = new Vector2((float)Math.Sin(Rotation) * currentPower, - (float)Math.Cos(Rotation) * currentPower + CurrentGravity);
            
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
        }

        public void DrawMe(SpriteBatch sp)
        {
            
            leftEngine.engineParticles.DrawMe(sp);
            rightEngine.engineParticles.DrawMe(sp);
            
            sp.Draw(texture, position, null, Color.White, Rotation, origin, 0.1f, SpriteEffects.None, 1f);
            shipGun.gun.DrawMe(sp);
            #region debug
            //DebugManager.DebugString("current power: " + currentPower, new Vector2(0, 0));
            //DebugManager.DebugString("Rotation: "+ Rotation, new Vector2(0, 22));    
            //DebugManager.DebugString("Planotation: "+PlanRotation, new Vector2(0, 44));
            //DebugManager.DebugString("Origin: " + origin, new Vector2(0, 66));

            // DebugManager.DebugString("LeftEngine distance: " + leftEngine.distance, new Vector2(0,0));
            #endregion
        }
    }
}

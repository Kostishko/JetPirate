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
        //TODO : put here all particle Systems

        //Movement variables (there are variable of real velocity and plan velocity to create the effect of inertion)
        //Rotation heritage from Object2D
        private float planRotation;
        protected float PlanRotation
        {
            get => planRotation;
            set
            {
                planRotation = Object2D.ModulasClamp(value, (float)-Math.PI * 2, (float)Math.PI * 2);
            }
        } // rotation that 
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


        public JetShip(Vector2 pos, float rot, Texture2D tex) : base(pos,rot)
        {
            texture = tex;
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
            maxGravity = 2f;
            maxPower = 4f;
        }


        public void UpdateMe(GamePadState gPad, GameTime gTime)
        {
            //Rotate and increse power with Triggers and decrease that if Triggers released
            if(gPad.Triggers.Left!=0)
            {
                PlanRotation -= 0.02f*gPad.Triggers.Left;
                LeftPower += 0.01f * gPad.Triggers.Left;
            }
            else
            {
                LeftPower -= 0.05f;
            }

            if(gPad.Triggers.Right!=0) 
            {
                PlanRotation += 0.02f * gPad.Triggers.Right;
                RightPower+=0.01f* gPad.Triggers.Right;
            }
            else
            {
                RightPower -= 0.05f;
            }

            //compute inertion of rotation
            if (Rotation!=PlanRotation)
            {
                if(Rotation>PlanRotation)
                {
                    Rotation = Math.Clamp(Rotation-(0.1f/currentPower),PlanRotation,(float)Math.PI*2);
                }
                else
                {
                    Rotation = Math.Clamp(Rotation+(0.1f/currentPower),-(float)Math.PI * 2, PlanRotation );
                }
            }

            //Gravitation compute
            CurrentGravity = maxGravity/ currentPower;

            // velocity that create player
            planVelocity = new Vector2((float)Math.Sin(Rotation) * currentPower, - (float)Math.Cos(Rotation) * currentPower + CurrentGravity);

            position += planVelocity;
        }

        public void DrawMe(SpriteBatch sp)
        {
            sp.Draw(texture, position, null, Color.White, Rotation, origin, 0.1f, SpriteEffects.None, 1f);
            DebugManager.DebugString("current power: " + currentPower, new Vector2(0, 0));
            DebugManager.DebugString("Rotation: "+ Rotation, new Vector2(0, 22));    
            DebugManager.DebugString("Plan rotation: "+PlanRotation, new Vector2(0, 44));
        }
    }
}

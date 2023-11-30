using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;



namespace JetPirate
{
    internal class JetShipTest2
    {

        protected Texture2D texture;
        public Vector2 position;

        public JetShipTest2(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;

            Power = 0;
            RealRotate = 0;
            PlanRotate = 0;
            jetVelocity = Vector2.Zero;
        }

        protected float realRotate;
        public float RealRotate
        {
            get => realRotate;
            set
            {
                realRotate = Object2D.ModulasClamp(value, (float)-Math.PI * 2, (float)Math.PI * 2);
            }
        }


        protected float planRotate;
        protected float PlanRotate
        {
            get => planRotate;
            set
            {
                planRotate = Object2D.ModulasClamp(value, -(float)Math.PI * 2, (float)Math.PI * 2);
            }
        }

        protected float power;
        protected float Power
        {
            get => power;
            set
            {
                power = Math.Clamp(value, 0, 4);
            }
        }

        protected float gravity;

        protected Vector2 jetVelocity;
        protected Vector2 realVelocity;
        protected float velX, velY;


        public void UpdateMe(KeyboardState keyboard, GameTime gameTime)
        {
            if (keyboard.IsKeyDown(Keys.A))
            {
                PlanRotate -= 0.02f;
                Power += 0.01f ;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                PlanRotate += 0.02f;
                Power += 0.01f; 
            }

            if (RealRotate != PlanRotate)
            {
                if (RealRotate > PlanRotate)
                {
                    RealRotate = Math.Clamp(RealRotate - 0.015f, PlanRotate - 0.2f, (float)Math.PI * 2);
                }
                else
                {
                    RealRotate = Math.Clamp(RealRotate + 0.015f, -(float)Math.PI * 2, PlanRotate + 0.2f);
                }
            }
            if (keyboard.IsKeyUp(Keys.A) && keyboard.IsKeyUp(Keys.D))
            {
                Power -= 0.02f; 
            }

            jetVelocity = new Vector2((float)Math.Sin(RealRotate) * Power, -(float)Math.Cos(RealRotate) * Power+1.5f);

            if (velX != jetVelocity.X)
            {
                if(velX>jetVelocity.X)
                {
                    velX = Math.Clamp(velX - 0.05f, jetVelocity.X - 0.02f, 10);
                }
                else
                {
                    velX = Math.Clamp(velX + 0.05f, -100, jetVelocity.X + 0.02f);
                }
            }
            if(velY != jetVelocity.Y) 
            {
                if(velY>jetVelocity.Y)
                {
                    velY = Math.Clamp(velY - 0.05f, jetVelocity.Y - 0.02f, 10);
                }
                else
                {
                    velY = Math.Clamp(velY + 0.05f, -100, jetVelocity.Y + 0.02f);
                }
            }
            realVelocity = new Vector2(velX, velY);
            
            position += realVelocity;

        }

        public void DrawMe(SpriteBatch sp)
        {
            sp.Draw(texture, position, null, Color.White, RealRotate, new Vector2(texture.Width / 2, texture.Height/2), 0.1f, SpriteEffects.None, 0);

            //DebugManager.DebugString("Rotation: " + RealRotate, Vector2.Zero);
            //DebugManager.DebugString("JetVelocity: " + jetVelocity, new Vector2(0, 22));
            //DebugManager.DebugString("RealVel: " + realVelocity, new Vector2(0, 44));
            //DebugManager.DebugString("Power: " + Power, new Vector2(0, 66));
            //DebugManager.DebugString("gravity: " + gravity, new Vector2(0, 88));
            
        }



    }
}

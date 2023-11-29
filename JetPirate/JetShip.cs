using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks.Sources;
using static System.Net.Mime.MediaTypeNames;


namespace JetPirate
{
    internal class JetShip
    {
        protected Texture2D texture;
        protected Rectangle mainRec;
        protected Vector2 position;

        protected Rectangle colRec; 

        protected Vector2 center;

        protected float power;

        protected float Power
        {
            get => power;
            set
            {
                power = Math.Clamp(value, 0, 3f);
            }
        }

        protected float rotateLeft;
        protected float RotateLeft
        {
            get => rotateLeft;
            set
            {
                rotateLeft = Object2D.ModulasClamp(value,(float) -Math.PI, (float)Math.PI);
            }
        }
        protected float rotateRight;
        protected float RotateRight
        {
            get => rotateRight;
            set
            {
                rotateRight = Object2D.ModulasClamp(value,(float) -Math.PI,(float)Math.PI);
            }
        }

        protected float newRotate;
        protected float rotate;
        protected float Rotate
        {
            get => rotate;
            set 
            {
                rotate = ModulasClamp(value, (float)-Math.PI*2, (float)Math.PI*2);
            }
        }

        protected Vector2 jetVelocity;
        protected Vector2 velocity;
        protected float velX;
        protected float velY;

        public JetShip(Texture2D tex, Vector2  pos)
        {
            this.texture = tex;
            position = pos;
            mainRec = new Rectangle((int)Math.Round(pos.X),(int)Math.Round(pos.Y),tex.Width/10,tex.Height/10);
            colRec = new Rectangle((int)Math.Round(pos.X - mainRec.Size.X / 2), (int)Math.Round(pos.Y - mainRec.Size.X / 2), tex.Width / 12, tex.Height / 12);

            velocity = Vector2.Zero;
            Rotate = 0 ;
            Power = 0;
            
        }

        public void UpdateMe(KeyboardState keys)
        {
            if (keys.IsKeyDown(Keys.A))
            {
                //Rotate -= 0.01f;
                RotateLeft = Math.Clamp(RotateLeft - 0.001f,(float)-Math.PI*2,0);
                //RotateRight = 0.002f;
                Power +=0.005f;
            }
            if (keys.IsKeyDown(Keys.D))
            {
                //Rotate += 0.01f;
                RotateRight = Math.Clamp(RotateRight + 0.001f,0, (float)Math.PI * 2);

               // RotateLeft = 0.002f;
                Power += 0.005f;
            }

            Rotate += RotateLeft + RotateRight;

            //RotateLeft = Math.Clamp(RotateLeft + 0.001f, -(float)Math.PI * 2, 0);
            //RotateRight=Math.Clamp(RotateRight - 0.001f,0, (float)Math.PI * 2);
           // RotateLeft = 0;
           // RotateRight = 0;

            jetVelocity = new Vector2((float)Math.Sin(Rotate)*Power,-(float)Math.Cos(Rotate)*Power +1.5f);

            if(Rotate!=newRotate)
            {
                if (Rotate > newRotate)
                {
                    Rotate =Math.Clamp(Rotate-0.005f,newRotate, (float)Math.PI*2);
                }
                else
                {
                    Rotate = Math.Clamp(Rotate + 0.005F, (float)-Math.PI * 2, newRotate);
                }
            }
            
            if(velX!=jetVelocity.X)
            {
                if (velX > jetVelocity.X)
                {
                    velX -= 0.05f;
                }
                else
                {
                    velX += 0.05f;
                }
            }
            if(velY!=jetVelocity.Y)
            {
                if(velY>jetVelocity.Y)
                {
                    velY -= 0.05f;
                }
                else
                {
                    velY += 0.05f;
                }
            }

            velocity = new Vector2(velX, velY);
            //velocity.Y += 0.02f;
            position += velocity;
            //position.Y += 1f;
            Vector2 temp = new Vector2(position.X-mainRec.Width/2+ texture.Width / 48, position.Y-mainRec.Height/2+texture.Height/48);
            colRec.Location = temp.ToPoint();
            mainRec.Location = position.ToPoint();
            //mainRec.Location += velocity.ToPoint();

            if (keys.IsKeyUp(Keys.A) && keys.IsKeyUp(Keys.D))
            {
                Power -= 0.01f;
            }
            
            

        }

        public void DrawMe(SpriteBatch sp)
        {
            //sp.Draw(texture, mainRec, null, Color.White,Rotate, mainRec.Center.ToVector2(), SpriteEffects.None,0f);
            sp.Draw(texture, position, null, Color.White, Rotate, new Vector2(texture.Width/2,texture.Height), 0.1f, SpriteEffects.None, 0);
           DebugManager.DebugRectangle(colRec);
            DebugManager.DebugString("Rotation: " + Rotate, Vector2.Zero);
            DebugManager.DebugString("JetVelocity: " + jetVelocity, new Vector2(0, 22));
            DebugManager.DebugString("Power: "+power, new Vector2(0, 44));
            DebugManager.DebugString("rotateLeft: " + RotateLeft, new Vector2(0, 66));
            DebugManager.DebugString("rotateRight: " + RotateRight, new Vector2(0, 88));
        }

        static public float ModulasClamp(float value, float min, float max )
        {
            //var modulas = MathF.Abs(rangeMax-rangeMin);
            //if ((value %= modulas) < 0f)
            //    value += modulas;
            //return Math.Clamp(value+Math.Min(rangeMin,rangeMax),min,max);

            float ret;
            if (value >= max)
            {
                ret = min + value % (max - min);
                return ret;
            }
            else if (value < min)
            {
                ret = (max) - Math.Abs(value % (max - min));
                return ret;
            }

            return value;

        }

    }
}

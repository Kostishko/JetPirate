using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;


namespace JetPirate
{


    public class Gun : Object2D
    {
        //Graphic
        private Texture2D texture;
        private Vector2 origin;

        //reload timers
        private float reloadTimer;
        private float ReloadTimer
        {
            get => reloadTimer;
            set
            {
                reloadTimer = Math.Clamp(value, 0, reloadTime);
            }
        }

        private float reloadTime;
        //magazine
        private int magCapacity;
        private int magValue; 

        //bullets
        private Bullet bullet;
        private List<Bullet> bullets;

        //movement
        private float targetRotation;
        private float TargetRotation
        {
            get => targetRotation;
            set
            {
                targetRotation = ModulasClamp(value, (float)-Math.PI, (float)Math.PI);
            }
        }


        public Gun(Vector2 pos, float rot, Texture2D tex, Texture2D bulletTex) : base(pos, rot) 
        {
            //visual
            texture = tex;
            origin = new Vector2(tex.Width / 2, tex.Height / 2);

            //reloading var
            ReloadTimer = 0;
            reloadTime = 10f;

            //magazine
            magCapacity = 10;
            magValue = 10;

            bullets = new List<Bullet>();
            //bullets creating
            for (int i = 0; i< magCapacity; i++) 
            {
                var bul = new Bullet(Vector2.Zero, 0f, bulletTex);
                bullets.Add(bul);
              
            }
        }

        public void UpdateMe(GamePadState curState, GamePadState oldState, Vector2 pos)
        {
            //Movement control
            if (curState.ThumbSticks.Left.X != 0 || curState.ThumbSticks.Left.Y != 0)
            {
                Rotation = (float)Math.Atan2(-curState.ThumbSticks.Left.Y, curState.ThumbSticks.Left.X);               
            }
            position = pos;

            //Fire Control
            if (curState.Buttons.X == ButtonState.Released && oldState.Buttons.X== ButtonState.Pressed && magValue>0)
            {
                    for(int i = 0; i<bullets.Count;i++)
                    {
                        if (!bullets[i].isPhysicActive)
                        {
                        bullets[i].BulletFly(position, Rotation, new Vector2((float)Math.Sin(Rotation+Math.PI/2) , -(float)Math.Cos(Rotation + Math.PI / 2)));
                        magValue--;
                        break;
                        }
                    }
            }

            //reload control
            //reload if mag is not full
            if(curState.Buttons.Y==ButtonState.Released&& oldState.Buttons.Y!=ButtonState.Pressed) 
            {
                if(magValue<magCapacity&&ReloadTimer==0)
                {
                    ReloadTimer = reloadTime;
                }
            }
            //automatic reload if mag is empty
            if (magValue == 0)
            {
                if (ReloadTimer == 0)
                {
                    ReloadTimer = reloadTime;                    
                }
                else
                {
                    ReloadTimer -= 0.1f;
                }
            }
            //reload completing
            if (magValue<magCapacity)
            {
                if (ReloadTimer==0)
                {
                    magValue = magCapacity;
                }
            }

            //bullets control
            for(int i=0; i<bullets.Count-1;i++) 
            {
                bullets[i].UpdateMe();
            }
            
        }

        public void DrawMe(SpriteBatch sp)
        {
            sp.Draw(texture, position, null, Color.White, Rotation, origin, 1f, SpriteEffects.None, 1);
            for (int i = 0; i < bullets.Count - 1; i++)
            {
                bullets[i].DrawMe(sp);
            }
        }

        /// <summary>
        /// Returns current magazine capacity
        /// </summary>
        /// <returns></returns>
        public int GetMag()
        {
           return magValue;
        }

        /// <summary>
        /// Returns current reload timer
        /// </summary>
        /// <returns></returns>
        public float GetReloadTimer()
        {
            return ReloadTimer;
        }

        /// <summary>
        /// Returns overall time of reloading
        /// </summary>
        /// <returns></returns>
        public float GetReloadTime()
        {
            return reloadTime;
        }

        /// <summary>
        /// Returns standart mag capacity
        /// </summary>
        /// <returns></returns>
        public int GetMagCapacity()
        {
            return magCapacity;
        }

    }

   

}

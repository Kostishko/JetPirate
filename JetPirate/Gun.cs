using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;

namespace JetPirate
{


    internal class Gun : Object2D
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
            origin = new Vector2(tex.Width / 6, tex.Height / 4);

            //reloading var
            ReloadTimer = 0;
            reloadTime = 3f;

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
            if (curState.Buttons.RightShoulder == ButtonState.Released && oldState.Buttons.RightShoulder == ButtonState.Pressed && magValue>0)
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
            sp.Draw(texture, position, null, Color.White, Rotation, origin, 0.5f, SpriteEffects.None, 1);
            for (int i = 0; i < bullets.Count - 1; i++)
            {
                bullets[i].DrawMe(sp);
            }
        }

    }

    internal class Bullet : Object2D
    {

        //Gun Characteristics
        private float damage;
        private float speed;
        private Vector2 velocity;

        //Visual
        private Texture2D texture;
        private Vector2 origin;

        //Flying timer
        private float flyTime;
        private float FlyTime
        {
            get => flyTime;
            set
            {
                flyTime = Math.Clamp(value, 0f, flyTimer);
            }
        }

        private float flyTimer;
        
        //texture should be squere
        public Bullet(Vector2 pos, float rot, Texture2D tex): base(pos, rot) 
        {


            physicRec = new Rectangle((int)Math.Round(pos.X), (int)Math.Round(pos.Y), (int)Math.Round(tex.Width*0.3f), (int)Math.Round(tex.Height * 0.3f));

            //visual
            texture = tex;
            origin = new Vector2(tex.Width*0.3f/2, tex.Height*0.3f/2);

            //gameplay
            damage = 50f;
            speed = 15f;

            //timer of bullet fly
            FlyTime = 0f;
            flyTimer = 15f;

            //physic
            PhysicManager.AddObject(this);
        }

        public override void Collided(Object2D obj)
        {

          if(obj is Enemy)
            {
                Enemy enemy = (Enemy)obj;
                enemy.TakeDamage(damage);
                this.isPhysicActive = false;
                FlyTime = 0;
            }
        }

        public void DrawMe(SpriteBatch sp)
        {
            if (isPhysicActive)
            { 
                sp.Draw(texture, position, null, Color.White, Rotation, origin, 0.3f, SpriteEffects.None, 1);
                DebugManager.DebugRectangle(physicRec);
            }
        }
        public void BulletFly(Vector2 pos, float rot, Vector2 vel)
        {
            isPhysicActive = true;
            Rotation = rot;
            position = pos;
            velocity= vel;
            FlyTime = flyTimer;
        }

        public void UpdateMe()
        {
            if (isPhysicActive) 
            {
                position += velocity * speed;
                physicRec.Location = position.ToPoint();
                FlyTime -= 0.1f;
                if(FlyTime==0)
                {
                    isPhysicActive=false;
                }
            }
        }



    }

}

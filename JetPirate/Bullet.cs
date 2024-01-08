using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetPirate
{
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
        public Bullet(Vector2 pos, float rot, Texture2D tex) : base(pos, rot)
        {


            physicRec = new Rectangle((int)Math.Round(pos.X), (int)Math.Round(pos.Y), (int)Math.Round(tex.Width / 2f), (int)Math.Round(tex.Height / 2f));

            //visual
            texture = tex;

            origin = new Vector2(tex.Width / 4, tex.Height / 4);

            //gameplay
            damage = 50f;
            speed = 5f;

            //timer of bullet fly
            FlyTime = 0f;
            flyTimer = 15f;

            //physic
            PhysicManager.AddObject(this);
        }

        public override void Collided(Object2D obj)
        {

            if (obj is Enemy)
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
                sp.Draw(texture, position, null, Color.White, Rotation, origin, 0.5f, SpriteEffects.None, 1);
                DebugManager.DebugRectangle(physicRec);
            }
        }
        public void BulletFly(Vector2 pos, float rot, Vector2 vel)
        {
            isPhysicActive = true;
            Rotation = rot;
            position = pos;
            velocity = vel;
            FlyTime = flyTimer;
        }

        public void UpdateMe()
        {
            if (isPhysicActive)
            {
                position += velocity * speed;
              //  physicRec.Location = position.ToPoint();
                FlyTime -= 0.1f;
                if (FlyTime == 0)
                {
                    isPhysicActive = false;
                }
            }
        }



    }
}

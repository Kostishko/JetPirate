using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JetPirate
{


    internal class Gun : Object2D
    {
        public float reloadTimer;
        public float reloadTime;
        public int magCapacity;

        private Bullet bullet;





        public Gun(Vector2 pos, float rot) : base(pos, rot) 
        {
            
        }




    }

    internal class Bullet : Object2D
    {
        public Rectangle colRec;
        public float Damage;

        //met enemy
        private Enemy enemy;

        public Bullet(Vector2 pos, float rot): base(pos, rot) 
        {

            PhysicManager.AddObject(this);
        }

        public new void Collided(Object2D obj)
        {
          if(obj is Enemy)
            {
                enemy = (Enemy)obj;
            }
        }



    }

}

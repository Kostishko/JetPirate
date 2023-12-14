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





        public Gun(Vector2 pos, float rot) : base(pos, rot) 
        {            
        }

        public void UpdateMe(GamePadState curState, GamePadState oldState)
        {
            if (curState.ThumbSticks.Left.X != 0 || curState.ThumbSticks.Left.Y != 0)
            {
                TargetRotation = (float)Math.Atan2(curState.ThumbSticks.Left.Y, curState.ThumbSticks.Left.X);
                if (Rotation!=TargetRotation)
                {
                    if( )
                    {

                    }
                }
            }
            
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

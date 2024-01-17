using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JetPirate
{
    internal class UIManager
    {

        //ContentManager
        private ContentManager contentUI;

        //Font
        private SpriteFont fontUI;

        //UI pictures
        private Texture2D shipHealthPic;
        private Texture2D bulletCounterPic;
        private Texture2D enemyPic;

        // to ancor ui elements
        private Camera cam;
        private Vector2 ancorUI;

        // current player's health and gun 
        private JetShip shipUI;
        private Gun gunUI;

        //Enemy manager
        private EnemyManager enemyManager;

        


        public UIManager(Camera cam, JetShip jetShip, ContentManager content, EnemyManager enemyManager) 
        {
            contentUI= content;
            fontUI = contentUI.Load<SpriteFont>("Fonts/MajorFont");
            this.cam = cam;
            shipUI = jetShip;
            gunUI = shipUI.GetGun();
            this.enemyManager = enemyManager;



            //UI pics
            shipHealthPic = contentUI.Load<Texture2D>("Sprites/Ship_03");
            bulletCounterPic = contentUI.Load<Texture2D>("Sprites/Bullet");
            enemyPic = contentUI.Load<Texture2D>("Sprites/Rocket");
        }


        public void UpdateMe()
        {
            //update an ancor
            ancorUI = -cam.position;
        }


        public void DrawMe(SpriteBatch sp)
        {
            //health points drawning
            sp.Draw(shipHealthPic, ancorUI + new Vector2(25, 50), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            sp.DrawString(fontUI, " X " + shipUI.GetHealth(), ancorUI + new Vector2(100, 45), Color.White);


            // draw all magazine capacity
            for (int i = 0; i < gunUI.GetMagCapacity(); i++)
            {
                if (i < gunUI.GetMag())
                {
                    sp.Draw(bulletCounterPic, ancorUI + new Vector2(50, 120 + i * 25), Color.White);
                }
                else
                {
                    sp.Draw(bulletCounterPic, ancorUI + new Vector2(50, 120 + i * 25), Color.Gray);
                }
            }

            //reload indicator
            if(gunUI.GetReloadTimer()>0&& gunUI.GetReloadTimer()!=gunUI.GetReloadTime())
            {
                sp.DrawString(fontUI, "RELOAD", ancorUI + new Vector2(25, 145), Color.Red, 0.5f, Vector2.Zero, 0.5f, SpriteEffects.None,0f);
            }

            sp.Draw(enemyPic, ancorUI + new Vector2(1100, 50), null, Color.White, (float)Math.PI/2 , Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            sp.DrawString(fontUI, " X " + enemyManager.enemyCounter, ancorUI + new Vector2(1100, 60), Color.White);

        }






    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetPirate
{
    internal class EnemyManager
    {
        //variables of waves
        public int enemyCounter;


        //Enemies
        private List<Enemy> enemies;

        //Visual
        private Texture2D texture;


        public EnemyManager(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/"); // fill with enemy spritesheet

            enemies = new List<Enemy>();


            //enemies creating 
            for (int i = 0; i < 30; i++)
            {
                enemies.Add(new Enemy(Vector2.Zero, 0f, this));
            }


        }


    }
}

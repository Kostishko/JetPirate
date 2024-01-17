using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace JetPirate
{
    internal class EnemyManager
    {
        
        


        //Enemies
        private List<Enemy> enemies;

        //Visual
        private Texture2D texture;
        private List<Rectangle> frameRectangles;

        //borders for enemy spawn
        private Camera camera;
        private Vector2 leftBorder;
        private Vector2 rightBorder;


        //Waves        
        private int freeEnemiesCounter; // destroyed or flyed away enemies (free for the next wave) 
        private int waveCounter; // how many waves are bitten
        private int waveEnemyCounter; // how many enemies in the next wave
        private float timeBetweenWave; // time between the next wave
        private float timerBetweenWave; // current time to next wave
        private float timeBetweenEnemies;// time between enemies due the wave
        private float timerBetweenEnemies; // current timer between enemies in the wave
        private float enemiesSpeed; //speed on the current wave
        
        public enum WaveType : byte// movement pattern for enemy
        {
            Target,
            Cosin,
            Round
        }
        private WaveType currentType;

        public enum WaveState :byte
        {
            pause,
            between,
            spawn
        }
        public WaveState currentState;





        public EnemyManager(ContentManager content, Camera camera)
        {
            texture = content.Load<Texture2D>("Sprites/"); // fill with enemy spritesheet

            enemies = new List<Enemy>();


            //enemies creating 
            for (int i = 0; i < 50; i++)
            {
                enemies.Add(new Enemy(Vector2.Zero, 0f, this));
            }

            this.camera = camera;
        }


        public void UpdateMe()
        {

            switch(currentState)
            {
                case WaveState.pause:
                    break;
                case WaveState.spawn:
                    break;
                case WaveState.between:
                    break;

            }

            //Borders for enemies
            leftBorder = new Vector2(camera.position.X - 640, camera.position.Y - 440);
            rightBorder = new Vector2(camera.position.X + 1860, camera.position.Y + 1160);



        }


        //sending the texture sheet to enemies
        public Texture2D GetTexture()
        {
            return texture;
        }


        //return borders for Enemies around
        public Vector2 GetLeftUpBorder()
        {
            return leftBorder;
        }

        public Vector2 GetRightDownBorder()
        {
            return rightBorder;
        }


    }
}

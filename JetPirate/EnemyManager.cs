using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace JetPirate
{
    /// <summary>
    /// Manager that controls enemy waves
    /// </summary>
    public class EnemyManager
    {


        //jet
        private JetShip jet;

        //Enemies
        private List<Enemy> enemies;

        //Visual
        private Texture2D texture;
       // private List<Rectangle> frameRectangles;

        //borders for enemy spawn
        private Camera camera;
        private Vector2 leftBorder;
        private Vector2 rightBorder;


        //Waves        
        private int waveCounter; // how many waves are bitten
        private int waveEnemyCounter; // how many enemies in the next wave
        private float timeBetweenWave; // time between the next wave
        private float timerBetweenWave; // current time to next wave
        private float timeBetweenEnemies;// time between enemies due the wave
        private float timerBetweenEnemies; // current timer between enemies in the wave
        private float enemiesSpeed; //speed on the current wave
        private int startPosShiftX;
        private int startPosShiftY;
        private Vector2 startPos;
        public int enemyCounter;

        private Random rng;

        public enum WaveState :byte
        {
            pause,
            between,
            spawn
        }
        public WaveState currentState;

        public EnemyManager(ContentManager content, Camera camera, JetShip jet) // made a mistake - I'd should put here Game1 and not cam\jet separately 
        {
            texture = content.Load<Texture2D>("Sprites/Rocket_02");
            this.jet = jet;

            enemies = new List<Enemy>();


            //enemies creating 
            for (int i = 0; i < 20; i++)
            {
                enemies.Add(new Enemy(Vector2.Zero, 0f, this, content));
            }

            this.camera = camera;
            rng = new Random();

            ResetMe();
        }


        public void UpdateMe()
        {

            switch(currentState)
            {
                case WaveState.pause:
                    break;
                case WaveState.spawn:
                    SpawnStateUpdate();
                    break;
                case WaveState.between:
                    BetweenStateUpdate();
                    break;

            }

            for(int i = 0;i < enemies.Count;i++)
            {
                enemies[i].UpdateMe();
            }

            //Borders for enemies
            leftBorder = new Vector2(jet.GetPosition().X - 1280, jet.GetPosition().Y - 880);
            rightBorder = new Vector2(jet.GetPosition().X + 1280, jet.GetPosition().Y + 880);



        }

        public void DrawMe(SpriteBatch sp)
        {
            for (int i = 0; i < enemies.Count;i++)
            {
                enemies[i].DrawMe(sp);
            }
        }


        public void BetweenStateUpdate()
        {
            if (timerBetweenWave>0)
            {
                timerBetweenWave -= 0.1f;
            }
            else
            {
                if (FreeEnemyCheck() >= 10)
                {
                    startPosShiftY =  rng.Next(0, 440);                    

                    startPosShiftX = rng.Next(0, 2) == 0? -1: 1;

                    enemiesSpeed = Math.Clamp(waveCounter * 1.6f, 6f, 10f);
                    currentState = WaveState.spawn;
                }
                
            }
        }

        public void SpawnStateUpdate()
        {
            startPos = new Vector2(jet.GetPosition().X+(startPosShiftX * 700), jet.GetPosition().Y+(startPosShiftY*startPosShiftX));
            if (waveEnemyCounter<10)
            {
                if (timerBetweenEnemies >= 0)
                {
                    timerBetweenEnemies -= 0.1f;
                }
                else
                {
                    GetFreeEnemy().ResetMe(enemiesSpeed, jet.GetPosition(), startPos, texture);
                    waveEnemyCounter++;
                    timerBetweenEnemies = timeBetweenEnemies;
                }
            }
            else
            {
                timerBetweenWave = timeBetweenWave;
                waveEnemyCounter = 0;
                currentState = WaveState.between;
            }

        }

        public int FreeEnemyCheck()
        {
            int count=0; 
            for (int i = 0;i<enemies.Count;i++)
            {
                if (!enemies[i].GetEnemyState())
                {
                    count++;
                }
            }
            return count;
        }

        public Enemy GetFreeEnemy()
        {
            for (int i = 0; i<enemies.Count;i++)
            {
                if (!enemies[i].GetEnemyState())
                {
                    return enemies[i];
                }
            }
            return null;
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

        public void ResetMe()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Destroyed();
            }
            waveCounter = 0;
            waveEnemyCounter = 0;
            timeBetweenWave = 15f;
            timerBetweenWave = 15f;
            timeBetweenEnemies = 4f;
            timerBetweenEnemies = 4f;
            enemiesSpeed=0;         
            enemyCounter=0;
            currentState = WaveState.between;
            
    }




    }
}

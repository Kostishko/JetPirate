using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace JetPirate
{
    internal class ParticleSystem
    {
        Texture2D texture;
        Random random;
        Vector2 position;
        Color color;
        bool fading;
        float startSize, endSize;
        float speed;
        int amount;
        Vector2 direction;

        public List<Particle> particles;
        public bool play;
        public float time;
        public float timeMax;
        public Queue<Particle> queueParticles;

        public ParticleSystem (Texture2D texture, Random rng, Vector2 position, Color color, bool fading, float startSize, float endSize, float speed, int amount, Vector2 direction, float timeMax )
        {
            this.texture = texture;
            this.random = rng;
            this.position = position;
            this.color = color;
            this.fading = fading;
            this.startSize = startSize;
            this.endSize = endSize;
            this.speed = speed;
            this.amount = amount;
            this.direction = direction;
            this.timeMax = timeMax;
            time = timeMax;

            particles = new List<Particle>();
            queueParticles = new Queue<Particle>();
            for (int i = 0; i < this.amount;i++)
            {
                particles.Add(new Particle(this.position, Vector2.One, this.texture, this.color, this.speed, this.startSize,this.endSize, this.fading));
            }

        }

        public void UpdateMe(GameTime gameTime)
        {
            if(play)
            {
               

            }

        }


    }

    class Particle
    {
        Vector2 direction;
        Vector2 position;
        Texture2D texture;
        Color color;
        float speed;
        bool fading;        
        float endSize;
        float curSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">Start position</param>
        /// <param name="direction">Only normolised vectors</param>
        /// <param name="texture">Image</param>
        /// <param name="color"></param>
        /// <param name="speed"></param>
        public Particle(Vector2 position, Vector2 direction, Texture2D texture, Color color, float speed, float startSize, float endSize, bool fading)
        {
            this.speed = speed;
            this.position = position;
            this.direction = direction;
            this.texture = texture;
            this.color = color;
            this.curSize = startSize;

        }

        public void UpdateMe()
        {
            //fading
            if(fading)
            {
                color.A--;
            }

            //size
            if (curSize > endSize)
            {
                curSize -= 0.1f;
            }
            else
            {
                curSize += 0.1f;
            }

            position += direction * speed;
           
        }

        public void DrawMe(SpriteBatch sp)
        {
            sp.Draw(texture, position, null, color, 0, Vector2.Zero, curSize,SpriteEffects.None,0);
        }

    }
}
